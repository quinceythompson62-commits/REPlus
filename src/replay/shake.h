// This file is part of RockstarEditorPlus.
// Copyright (C) 2026 CoreFX (crxhvrd@proton.me)
// SPDX-License-Identifier: GPL-3.0-only
// RockstarEditorPlus is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License v3 as published by the Free
// Software Foundation. See the LICENSE file for details.

#pragma once
#include <cstdint>
#include <cmath>

// =============================================================================
//  Procedural camera shake
// =============================================================================
//  Two independent layers, because one fractal sum cannot express what camera
//  shake actually looks like:
//
//    SWAY   - low frequency, larger amplitude. The operator breathing, weight
//             shifting, a boom settling. This is what sells "handheld".
//    JITTER - high frequency, small amplitude. Engine buzz, footfall, rattle.
//
//  Octaves add detail *within* a layer at a fixed half-amplitude falloff, which
//  is not the same thing: sway and jitter need independent amplitude AND rate,
//  and their ratio is most of the character of a shake.
//
//  Every term is directly settable, per axis and per marker. There are no
//  presets - a preset is just a set of numbers you happened to like.
//
//  THE important property: it is a pure function of replay time. No running
//  RNG, no frame counter, no accumulated state. That gives:
//
//    * preview matches export, exactly
//    * scrubbing back and forth reproduces the same shake every pass
//    * a re-render reproduces the take you approved
//    * pausing freezes the shake instead of leaving a still frame jittering
//    * playback speed and fine-scrubbing stay correct with no special cases
//
//  Seed exists so you can reroll a take without changing its character.
// =============================================================================
namespace shake
{
	struct Layer
	{
		float posAmp = 0.0f;  // metres
		float rotAmp = 0.0f;  // degrees
		float freq   = 1.0f;  // oscillations per second, roughly
	};

	// -------------------------------------------------------------------------
	// SIMPLE mode.
	//
	// Four amplitudes in two units is the honest model, and it is the wrong
	// first experience: the mod shipped with all four at zero, so shake did
	// nothing until you found and set several numbers, and the pos:rot ratio -
	// the thing that actually decides whether a shake reads as an operator or
	// as an effect - was yours to get wrong.
	//
	// Simple mode collapses that to ONE intensity, expanded through a fixed
	// ratio below. The ratio is the opinion: 0.05 m of translation per degree
	// of rotation, and a 70/30 split between the slow and fast layers. Those
	// numbers are not derived from anything - they are what handheld looks
	// like, chosen once so the user does not have to choose them.
	//
	// Complex mode is untouched and remains the full model.
	// -------------------------------------------------------------------------
	namespace simple
	{
		// UNITS, and they are the sibling Simple Camera project's, deliberately:
		// amplitude 1.0 means ONE DEGREE of rotation and five centimetres of
		// translation. Its shake evaluates
		//
		//     posMag = amp * 0.05      rotMag = amp * 1.0
		//
		// so a value carried across from a shot authored there means the same
		// thing here. The four constants below therefore sum to exactly 1.0 deg
		// and 0.05 m, and the 70/30 split between the slow and fast layers is
		// what divides them.
		//
		// The split is the opinion - it is what handheld looks like, chosen once
		// so the user does not have to. The SCALE is not an opinion any more; it
		// is a unit, and "amplitude 2.0" now means two degrees rather than
		// whatever 2.0 used to expand to.
		inline constexpr float kSwayPos   = 0.0350f;  // metres   } sum = 0.05 m
		inline constexpr float kJitterPos = 0.0150f;  //          }
		inline constexpr float kSwayRot   = 0.700f;   // degrees  } sum = 1.0 deg
		inline constexpr float kJitterRot = 0.300f;   //          }

		// Sway is SLOW - body drift, not a wobble - and its rate is now the
		// frequency the user types, in HERTZ, rather than a multiplier on a
		// hidden constant. 0.35 Hz is a three-second wander; 0.20 is slower
		// still and is what the shot this was rebuilt for uses.
		//
		// Jitter rides above it at a fixed ratio. Real handheld is a sub-hertz
		// wander with rattle on top, so the two layers want to be an order of
		// magnitude apart - a factor of five puts them in the same busy mid-band
		// and is most of why an even-sounding shake reads as fake.
		inline constexpr float kJitterFreqRatio = 22.9f;   // 8.0 / 0.35, kept
	}

	// -------------------------------------------------------------------------
	// STYLES (Simple mode).
	//
	// A style is the part of a shake that is NOT a number you want to type:
	// how the movement splits between the slow and fast layers, how far apart
	// their rates sit, which axes it lives on, and whether there are footsteps
	// in it. Amplitude and Frequency stay the two rows you tune; picking a
	// style in the menu also drops its suggested values into them as a
	// starting point.
	//
	// Style 0, Classic, is the exact pre-style behaviour: same split, same
	// ratio, axis multipliers of 1. A project saved before styles existed has
	// no style key, inherits Classic, and renders bit-for-bit as it did.
	//
	// Every multiplier here is applied ON TOP of the user's axis weights, so
	// the Axes page still works in every style.
	// -------------------------------------------------------------------------
	struct StyleDef
	{
		const char* name;
		// Split of one amplitude unit. Positions in metres, rotations in degrees.
		float swayPos, jitPos, swayRot, jitRot;
		float jitRatio;          // jitter rate = sway rate * this
		float axisPos[3];        // lateral, forward, vertical multipliers
		float axisRot[3];        // pitch, roll, yaw multipliers
		// Footsteps. Rate = sway rate * bobRatio. Zero bobPos and bobRoll = none.
		float bobPos;            // metres of vertical bob per amplitude unit
		float bobRoll;           // degrees of side-to-side roll per amplitude unit
		float bobRatio;
		// Suggested starting values written into the marker when picked.
		float defAmp, defFreq, defVariation;
		const char* help;
	};

	inline constexpr StyleDef kStyles[] = {
		{ "Classic",
		  0.0350f, 0.0150f, 0.700f, 0.300f, 22.9f,
		  {1.0f, 1.0f, 1.0f}, {1.0f, 1.0f, 1.0f},
		  0.0f, 0.0f, 0.0f,
		  2.0f, 0.20f, 0.7f,
		  "The original Rockstar Editor+ shake. Old projects use this." },
		{ "Handheld",
		  0.0350f, 0.0150f, 0.720f, 0.280f, 20.0f,
		  {1.0f, 0.8f, 1.0f}, {1.0f, 1.15f, 0.9f},
		  0.0f, 0.0f, 0.0f,
		  2.2f, 0.22f, 0.7f,
		  "Operator holding the camera, standing. Slow wander with a little rattle." },
		{ "Shoulder Rig",
		  0.0300f, 0.0150f, 0.800f, 0.200f, 14.0f,
		  {0.8f, 0.6f, 1.1f}, {0.7f, 1.2f, 0.8f},
		  0.0f, 0.0f, 0.0f,
		  1.3f, 0.25f, 0.6f,
		  "Heavier camera on the shoulder: steadier, breathes up and down, leans in roll." },
		{ "Walking",
		  0.0200f, 0.0100f, 0.550f, 0.150f, 18.0f,
		  {1.0f, 0.6f, 0.8f}, {1.0f, 0.9f, 0.9f},
		  0.0120f, 0.25f, 6.3f,
		  1.5f, 0.30f, 0.5f,
		  "Operator walking with the camera. Adds footstep bob and side-to-side sway." },
		{ "Running",
		  0.0250f, 0.0200f, 0.600f, 0.350f, 16.0f,
		  {1.0f, 0.7f, 1.0f}, {1.1f, 1.0f, 1.0f},
		  0.0180f, 0.35f, 6.0f,
		  2.8f, 0.45f, 0.4f,
		  "Running behind the action. Hard footfalls and a lot of energy." },
		{ "Vehicle",
		  0.0150f, 0.0350f, 0.250f, 0.750f, 30.0f,
		  {0.5f, 0.3f, 1.0f}, {1.0f, 0.6f, 0.5f},
		  0.0f, 0.0f, 0.0f,
		  0.6f, 0.50f, 0.6f,
		  "Camera mounted on a car. Road buzz, mostly up-and-down and pitch." },
		{ "Helicopter",
		  0.0400f, 0.0100f, 0.800f, 0.200f, 40.0f,
		  {1.0f, 0.5f, 0.9f}, {0.8f, 1.0f, 0.6f},
		  0.0f, 0.0f, 0.0f,
		  1.4f, 0.15f, 0.8f,
		  "Aerial from a chopper. Big slow drift with rotor vibration on top." },
		{ "Drone",
		  0.0450f, 0.0050f, 0.900f, 0.100f, 25.0f,
		  {1.0f, 0.5f, 1.0f}, {0.7f, 0.8f, 1.0f},
		  0.0f, 0.0f, 0.0f,
		  0.7f, 0.12f, 0.6f,
		  "Stabilised drone. Smooth floating drift, almost no buzz." },
		{ "Tripod Breath",
		  0.0400f, 0.0100f, 0.950f, 0.050f, 12.0f,
		  {1.0f, 0.5f, 1.0f}, {1.0f, 0.8f, 1.0f},
		  0.0f, 0.0f, 0.0f,
		  0.15f, 0.25f, 0.5f,
		  "Barely there. Makes a locked-off shot feel alive without looking shaky." },
		{ "Earthquake",
		  0.0200f, 0.0300f, 0.300f, 0.700f, 10.0f,
		  {0.8f, 0.4f, 1.0f}, {1.0f, 1.0f, 0.7f},
		  0.0f, 0.0f, 0.0f,
		  2.5f, 0.80f, 1.0f,
		  "Violent ground shake. Pair with Impact on the marker for a big hit." },
	};
	inline constexpr int kStyleCount = (int)(sizeof(kStyles) / sizeof(*kStyles));

	inline const StyleDef& style(int i)
	{
		if (i < 0 || i >= kStyleCount) i = 0;
		return kStyles[i];
	}

	struct Params
	{
		Layer sway  { 0.0f, 0.0f, 1.8f };
		Layer jitter{ 0.0f, 0.0f, 9.0f };

		int octaves = 3;   // detail within each layer, 1-6
		int seed    = 0;   // reroll without changing character

		// How unevenly the shake plays out over time. 0 is the old behaviour -
		// statistically identical second after second, which is exactly what
		// "too uniform" was. Higher detunes the six channels against each other
		// and adds a slow swell/lull envelope over the whole thing.
		float variation = 0.7f;

		// Per-axis weights on top of the amplitudes.
		// axisPos = { lateral, forward, vertical }
		// axisRot = { pitch,   roll,    yaw      }
		//
		// Rotations are EQUAL by default. Roll used to ship at 0.5 and that was
		// a mistake twice over: it is the axis a handheld rig moves in most
		// obviously, and it is also the one that reads WEAKEST per degree -
		// pitch and yaw shift the whole frame, while roll leaves the centre
		// still and only moves the corners. Damping it was backwards.
		//
		// Forward translation keeps its 0.6: unlike the others it reads as a
		// zoom pump rather than as camera movement.
		float axisPos[3] = { 1.0f, 0.6f, 1.0f };
		float axisRot[3] = { 1.0f, 1.0f, 1.0f };

		// --- Simple mode ---
		bool  simpleMode = true;   // the default a new user meets
		float amplitude  = 1.0f;   // DEGREES of rotation (and 0.05 m per unit)
		float frequency  = 0.35f;  // HERTZ, the slow layer's rate
		int   style      = 0;      // index into kStyles; 0 = Classic

		// Footsteps, filled in by resolve() from the style. Zero = none.
		float bobPos  = 0.0f;      // metres
		float bobRoll = 0.0f;      // degrees
		float bobFreq = 0.0f;      // footfalls per second

		// --- Speed coupling (BOTH modes) ---
		//
		// A rig shakes harder when it is moving, and a camera that shakes the
		// same amount standing still is the single thing that reads as "effect"
		// rather than "operator".
		//
		// The speed fed in is the camera's speed along its own path at this
		// project time - a property of the marker layout and the speed profile,
		// NOT a frame-to-frame measurement. So this stays a pure function of
		// time: scrub to t, get the same speed, get the same shake, and a
		// re-render still matches the take you approved.
		float speedAmp     = 0.0f;   // extra amplitude at reference speed
		float speedFreq    = 0.0f;   // extra frequency at reference speed
		float speedRef     = 30.0f;  // m/s that counts as "fully moving"
		bool  stopWhenStill = false; // fade out when the camera is parked

		bool active() const
		{
			if (simpleMode) return amplitude > 0.0f;
			return sway.posAmp   > 0.0f || sway.rotAmp   > 0.0f ||
			       jitter.posAmp > 0.0f || jitter.rotAmp > 0.0f;
		}

		// Expand simple mode into the layer model everything downstream uses.
		// A no-op in complex mode, so callers can apply it unconditionally.
		// resolve() multiplies the axis weights by the style's, so running it
		// twice on one object would apply the style twice. evaluate() works on
		// a copy and the debug log resolves its own copy, but the guard makes
		// the mistake impossible rather than merely avoided.
		bool resolved = false;

		void resolve()
		{
			if (!simpleMode || resolved) return;
			resolved = true;
			const StyleDef& st = shake::style(style);
			sway.posAmp   = amplitude * st.swayPos;
			sway.rotAmp   = amplitude * st.swayRot;
			jitter.posAmp = amplitude * st.jitPos;
			jitter.rotAmp = amplitude * st.jitRot;

			// The typed frequency IS the sway's rate in Hz; jitter tracks it.
			sway.freq     = frequency;
			jitter.freq   = frequency * st.jitRatio;

			for (int i = 0; i < 3; ++i)
			{
				axisPos[i] *= st.axisPos[i];
				axisRot[i] *= st.axisRot[i];
			}

			bobPos  = amplitude * st.bobPos;
			bobRoll = amplitude * st.bobRoll;
			bobFreq = frequency * st.bobRatio;
		}
	};

	namespace detail
	{
		// Integer hash -> [-1,1]. Cheap, and reproducible on every machine and
		// every run, which is the entire point.
		inline float hash11(uint32_t n)
		{
			n = (n << 13) ^ n;
			n = n * (n * n * 15731u + 789221u) + 1376312589u;
			return (float)(n & 0x7FFFFFFFu) / (float)0x7FFFFFFF * 2.0f - 1.0f;
		}

		// Smooth value noise: hashed lattice, smoothstep between samples.
		// Smoothstep rather than linear so the derivative is continuous -
		// linear leaves a visible tick at every lattice crossing.
		inline float valueNoise(uint32_t seed, float x)
		{
			const float fl = floorf(x);
			const int   i  = (int)fl;
			const float f  = x - fl;

			const float a = hash11((uint32_t)(i)     * 374761393u + seed);
			const float b = hash11((uint32_t)(i + 1) * 374761393u + seed);

			const float u = f * f * (3.0f - 2.0f * f);
			return a + (b - a) * u;
		}

		// Fractal sum. Each octave doubles rate and halves amplitude; 2.03
		// rather than exactly 2 stops the octaves periodically re-aligning into
		// a visible pulse.
		//
		// Normalised by RMS, not by the sum of the amplitudes.
		//
		// Dividing by the sum (1.75 for three octaves) is what everyone writes
		// and it is wrong for this: three quasi-independent values almost never
		// align, so the result clustered around +-0.26 of its stated range and
		// the extremes were effectively unreachable. That is the "grey mush"
		// fBm is known for, and on a camera it reads as a shake with no
		// dynamics - always present, never going anywhere.
		//
		// sqrt(sum of squares) is the standard deviation of the sum, so this
		// puts the output in roughly the same statistical range as a single
		// octave. Peaks now happen. They can exceed 1 occasionally, which is
		// correct - a real shake has the odd big one - and callers clamp
		// nothing, so an amplitude is a typical excursion rather than a hard
		// ceiling.
		inline float fbm(uint32_t seed, float x, int octaves)
		{
			if (octaves < 1) octaves = 1;
			float sum = 0.0f, amp = 1.0f, sumSq = 0.0f;
			for (int o = 0; o < octaves; ++o)
			{
				sum   += valueNoise(seed + (uint32_t)o * 7919u, x) * amp;
				sumSq += amp * amp;
				x     *= 2.03f;
				amp   *= 0.5f;
			}
			return sumSq > 0.0f ? sum / sqrtf(sumSq) : 0.0f;
		}

		// -------------------------------------------------------------------
		// Summed sines - the "handheld" model. Ported from Simple Camera.
		//
		// Same spectrum as the fBm above: 2.03 between octaves and a 0.5
		// amplitude falloff, which at three octaves is 0.571 / 0.286 / 0.143 -
		// exactly Simple Camera's 4/7, 2/7, 1/7. The octave STRUCTURE was never
		// the difference between them.
		//
		// What differs is the waveform. A sine oscillates: it swings out, slows,
		// reverses, follows through. Value noise wanders - a random walk with no
		// commitment to where it was going. An operator's sway is a damped mass
		// with momentum, so it behaves like the former, and that is the whole of
		// why one reads as handheld and the other as drift.
		//
		// Normalised by the SUM here, not the RMS. The opposite of the noise
		// version, and correct for the same underlying reason: sines are
		// deterministic and periodically do all line up, so the sum is a real
		// peak that gets reached. Independent noise octaves almost never align,
		// which is why dividing those by the sum buried them near zero.
		//
		// Phase offsets from the seed rather than rand(), so it stays exactly
		// reproducible - Simple Camera can afford randomness because it never
		// has to render the same take twice.
		// -------------------------------------------------------------------
		inline float sineFbm(uint32_t seed, float x, int octaves)
		{
			if (octaves < 1) octaves = 1;
			if (octaves > 6) octaves = 6;

			// Simple Camera's first three, continued with values that keep the
			// octaves from sharing a rational ratio.
			static const float kPhaseMul[6] = { 1.0f, 1.7f, 2.3f, 3.1f, 4.3f, 5.9f };

			// 0..1000 radians, matching the scale its rand() seeds produced.
			const float ph = (hash11(seed * 2246822519u) * 0.5f + 0.5f) * 1000.0f;

			float sum = 0.0f, amp = 1.0f, total = 0.0f, rate = 1.0f;
			for (int o = 0; o < octaves; ++o)
			{
				sum   += sinf(x * rate + ph * kPhaseMul[o]) * amp;
				total += amp;
				rate  *= 2.03f;
				amp   *= 0.5f;
			}
			return total > 0.0f ? sum / total : 0.0f;
		}

		// Deterministic per-channel frequency spread.
		//
		// The one thing SimpleCamera does that REP did not: run each degree of
		// freedom at a slightly different rate. Six channels at identical rates
		// stay statistically identical forever - different numbers, same
		// texture. Detuned, they beat against each other over tens of seconds,
		// so the composite swells and settles on its own without anything
		// modulating it.
		//
		// Derived from the seed rather than rand(), so it is still exactly
		// reproducible - which is the whole reason REP can render.
		inline float channelRate(uint32_t seed, float spread)
		{
			return 1.0f + hash11(seed * 2654435761u) * spread;
		}

		inline void addLayer(const Layer& L, int octaves,
		                     uint32_t seedBase, float timeMs, float variation,
		                     const float axP[3], const float axR[3],
		                     float outPos[3], float outRot[3])
		{
			if (L.posAmp <= 0.0f && L.rotAmp <= 0.0f) return;

			// sineFbm wants radians; the layer rate is in Hz.
			constexpr float kTwoPi = 6.28318531f;
			const float secs = timeMs / 1000.0f;
			const float t    = secs * (L.freq > 0.0f ? L.freq : 1.0f) * kTwoPi;

			// +-22.5%, unconditionally rather than scaled by variation: the six
			// channels beating against each other IS the character, not an
			// embellishment on top of it.
			constexpr float kSpread = 0.225f;
			(void)variation;

			for (int i = 0; i < 3; ++i)
			{
				const uint32_t sp = seedBase + 0x01u + (uint32_t)i;
				const uint32_t sr = seedBase + 0x11u + (uint32_t)i;

				outPos[i] += sineFbm(sp, t * channelRate(sp, kSpread), octaves)
				           * L.posAmp * axP[i];
				outRot[i] += sineFbm(sr, t * channelRate(sr, kSpread), octaves)
				           * L.rotAmp * axR[i];
			}
		}

		// A slow gain on the whole shake: the operator settling, then shifting,
		// then settling again. ~0.09 Hz, so a swell lasts ten seconds or so.
		//
		// Deliberately shared by both layers and all six channels - one
		// envelope reads as one person breathing, whereas per-channel envelopes
		// just average out to the flat signal this exists to remove.
		//
		// Runs on absolute seconds rather than layer-scaled time so changing
		// Shake Speed does not also speed up the breathing.
		inline float envelope(uint32_t seed, float timeMs, float variation)
		{
			if (variation <= 0.0f) return 1.0f;
			const float n = fbm(seed + 0x5BF0u, (timeMs / 1000.0f) * 0.09f, 2);
			const float g = 1.0f + n * 0.85f * variation;
			return g < 0.05f ? 0.05f : g;
		}
	}

	struct Params;
	namespace detail
	{
		// Footsteps for the Walking / Running styles.
		//
		// phase counts footfalls. |sin| bottoms out with a sharp corner at each
		// heel strike and rises smoothly in between, which is what a walking
		// camera does - a plain sine bob reads as floating. Roll and lateral
		// drift run at half that rate, because weight moves left-right-left
		// once per TWO steps. Each step is a little different in size, seeded,
		// so it stays a pure function of time.
		inline void addFootsteps(const Params& p, uint32_t seed, float timeMs,
		                         float outPos[3], float outRotDeg[3]);
	}

	inline void detail::addFootsteps(const Params& p, uint32_t seed, float timeMs,
	                                 float outPos[3], float outRotDeg[3])
	{
		if (p.bobFreq <= 0.0f || (p.bobPos <= 0.0f && p.bobRoll <= 0.0f)) return;

		constexpr float kPi = 3.14159265f;
		const float phase = (timeMs / 1000.0f) * p.bobFreq
		                  + (hash11(seed + 0xB0Bu) * 0.5f + 0.5f);

		// Per-step size, +-20%, smooth from one step to the next.
		const float stepVar = 1.0f + 0.2f * valueNoise(seed + 0x57E9u, phase);

		const float s      = sinf(kPi * phase);
		const float dip    = fabsf(s) - 0.63662f;   // minus its mean, so no drift
		const float sway   = sinf(kPi * phase + 0.4f);

		outPos[2]    += p.bobPos * stepVar * dip * p.axisPos[2];
		outPos[0]    += p.bobPos * 0.6f * sway * p.axisPos[0];
		outPos[1]    += p.bobPos * 0.25f * cosf(2.0f * kPi * phase) * p.axisPos[1];
		outRotDeg[1] += p.bobRoll * stepVar * sway * p.axisRot[1];
		outRotDeg[0] += p.bobRoll * 0.35f * stepVar * dip * p.axisRot[0];
	}

	// Evaluate at a replay time. Outputs are CAMERA-LOCAL:
	//   outPos    = { right, forward, up }   metres
	//   outRotDeg = { pitch, roll,    yaw }  degrees about those axes
	//
	// speedMps is the camera's speed along its path AT THIS TIME. Pass a
	// NEGATIVE value for "not known" - the coupling and the motion gate are
	// then both skipped, and the result is the plain uncoupled shake. Zero is
	// a real speed and means the camera is parked, so the two cannot share a
	// value: a caller that does not know the speed must not be able to
	// accidentally silence the shake.
	//
	// `p` is taken BY VALUE and resolved here, so a caller cannot forget to
	// expand simple mode.
	inline void evaluate(Params p, float timeMs, float speedMps,
	                     float outPos[3], float outRotDeg[3])
	{
		outPos[0] = outPos[1] = outPos[2] = 0.0f;
		outRotDeg[0] = outRotDeg[1] = outRotDeg[2] = 0.0f;
		if (!p.active()) return;
		p.resolve();

		const bool haveSpeed = speedMps >= 0.0f;

		// --- speed coupling ---------------------------------------------
		float sn = 0.0f;
		if (haveSpeed && p.speedRef > 0.0f && speedMps > 0.0f)
		{
			sn = speedMps / p.speedRef;
			if (sn > 1.0f) sn = 1.0f;
		}

		const float ampGain  = 1.0f + p.speedAmp  * sn;

		// Motion gate. Smoothstepped over a speed BAND rather than switched at
		// a threshold, which is what removes the pop - and because the input is
		// position on a curve rather than a measurement, no attack/release
		// smoothing is needed. A live rig has to guess where the motion is
		// going; this already knows.
		float gate = 1.0f;
		if (p.stopWhenStill && haveSpeed)
		{
			constexpr float kLo = 0.05f;   // m/s: parked
			constexpr float kHi = 0.50f;   // m/s: definitely moving
			float g = (speedMps - kLo) / (kHi - kLo);
			if (g < 0.0f) g = 0.0f;
			if (g > 1.0f) g = 1.0f;
			gate = g * g * (3.0f - 2.0f * g);
			if (gate <= 0.0f) return;
		}

		// Distinct seed bases so the layers never correlate, and so changing
		// seed rerolls both together.
		const uint32_t s = (uint32_t)p.seed * 1013904223u;

		float var = p.variation;
		if (var < 0.0f) var = 0.0f;
		if (var > 1.0f) var = 1.0f;

		// One envelope over everything, folded in with the speed gain.
		const float aScale = ampGain * gate * detail::envelope(s, timeMs, var);

		p.sway.posAmp   *= aScale;  p.sway.rotAmp   *= aScale;
		p.jitter.posAmp *= aScale;  p.jitter.rotAmp *= aScale;
		p.bobPos        *= aScale;  p.bobRoll       *= aScale;

		// --- speed -> rate, WITHOUT a phase jump -----------------------------
		//
		// The old code multiplied the layer frequencies by the speed gain. The
		// shake is sin(time * freq), so changing freq at time t moves the phase
		// by t * dfreq - forty seconds into a clip, a 10% rate change swept the
		// phase through several whole cycles in a few frames. That is the
		// "camera whips for a moment when it starts moving" artefact.
		//
		// The shake still has to be a pure function of time, so the rate cannot
		// be integrated. Instead two copies run at FIXED rates - resting and
		// full-speed - and the speed crossfades between them. Neither phase ever
		// moves. The pair is close to uncorrelated, so the mix is equal-power
		// normalised to keep the size constant through the fade.
		const float rateHi = 1.0f + (p.speedFreq > 0.0f ? p.speedFreq : 0.0f);
		const float mixHi  = (rateHi > 1.0f) ? sn : 0.0f;

		auto run = [&](float rate, float gain)
		{
			if (gain <= 0.0f) return;
			Params q = p;
			q.sway.freq   *= rate;
			q.jitter.freq *= rate;
			q.bobFreq     *= rate;
			q.sway.posAmp   *= gain;  q.sway.rotAmp   *= gain;
			q.jitter.posAmp *= gain;  q.jitter.rotAmp *= gain;
			q.bobPos        *= gain;  q.bobRoll       *= gain;

			detail::addLayer(q.sway,   q.octaves, s + 0x0000u, timeMs, var,
				q.axisPos, q.axisRot, outPos, outRotDeg);
			detail::addLayer(q.jitter, q.octaves, s + 0x9E37u, timeMs, var,
				q.axisPos, q.axisRot, outPos, outRotDeg);
			detail::addFootsteps(q, s, timeMs, outPos, outRotDeg);
		};

		if (mixHi <= 0.0f)
		{
			run(1.0f, 1.0f);
		}
		else
		{
			const float a = 1.0f - mixHi, b = mixHi;
			const float norm = 1.0f / sqrtf(a * a + b * b);
			run(1.0f,   a * norm);
			run(rateHi, b * norm);
		}
	}

	// -------------------------------------------------------------------------
	// IMPACT - a one-off jolt that starts at a marker.
	//
	// A hit, a landing, an explosion going off: the camera gets kicked, rings
	// for a moment and settles. `strength` is the first swing in degrees.
	// dtMs is time since the marker; outside 0..2.5 s it adds nothing.
	//
	// Still a pure function of time - scrubbing to the same frame gives the
	// same jolt - and the seed picks which way it kicks.
	// -------------------------------------------------------------------------
	inline void addImpact(float strength, int seed, float dtMs,
	                      float outPos[3], float outRotDeg[3])
	{
		if (strength <= 0.0f || dtMs < 0.0f) return;
		const float t = dtMs / 1000.0f;
		if (t > 2.5f) return;

		constexpr float kTwoPi = 6.28318531f;
		// Fast attack (no pop on the first frame), exponential ring-down.
		const float env = (1.0f - expf(-t * 60.0f)) * expf(-t * 4.0f);

		const uint32_t s = (uint32_t)seed * 2654435761u + 0xC0FFEEu;
		const float dirP = detail::hash11(s)        < 0.0f ? -1.0f : 1.0f;
		const float dirR = detail::hash11(s + 7u);   // -1..1, roll can go either way
		const float dirY = detail::hash11(s + 13u);

		const float a = strength * env;
		outRotDeg[0] += a * dirP        * sinf(kTwoPi * 7.0f * t);
		outRotDeg[1] += a * 0.55f * dirR * sinf(kTwoPi * 5.3f * t + 1.0f);
		outRotDeg[2] += a * 0.30f * dirY * sinf(kTwoPi * 4.1f * t + 2.0f);
		outPos[2]    += a * 0.015f * dirP * sinf(kTwoPi * 7.0f * t + 0.5f);
		outPos[0]    += a * 0.008f * dirY * sinf(kTwoPi * 5.9f * t + 1.3f);
	}
}
