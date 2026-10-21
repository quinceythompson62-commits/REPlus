using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;

namespace CutscenesMenu;

public class Class1 : Script
{
	public class Cutscene
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public Cutscene()
		{
		}

		public Cutscene(string N, string D)
		{
			Name = N;
			Description = D;
		}
	}

	public string CurrentPlayingCutscene;

	private Vector3 StartPos;

	public List<string> CutscenePed1Comp = new List<string>
	{
		"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
		"10_0_0", "11_0_0"
	};

	public List<string> CutscenePed2Comp = new List<string>
	{
		"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
		"10_0_0", "11_0_0"
	};

	public List<string> CutscenePed3Comp = new List<string>
	{
		"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
		"10_0_0", "11_0_0"
	};

	public List<string> CutscenePed4Comp = new List<string>
	{
		"0_0_0", "1_0_0", "2_0_0", "3_0_0", "4_0_0", "5_0_0", "6_0_0", "7_0_0", "8_0_0", "9_0_0",
		"10_0_0", "11_0_0"
	};

	public string CurrentCutsene;

	public int cutscene;

	public Ped NonCutscene2;

	public Ped NonCutscene3;

	public Ped NonCutscene4;

	public bool SetCutsceneFadeout;

	public bool TeleportBacktoStartPos;

	public Model PlayerModel;

	public List<string> Known_Entity_names = new List<string> { "ABIGAIL", "FRANKLIN", "LESTER", "MICHAEL", "GUNMAN_SELECTION_1", "GUNMAN_SELECTION_2", "DEVON", "MOLLY", "TREVOR", "MOLLY" };

	public Ped CutsenePed1;

	public Ped CutsenePed2;

	public Ped CutsenePed3;

	public Ped CutsenePed4;

	public bool PlayingCutscene;

	public bool RepalceMp1;

	public bool RepalceMpMFwithPlayer;

	public bool HideNonMainChars;

	public List<Ped> NonMainChars = new List<Ped>();

	public bool RepalceMp2;

	public bool RepalceMp3;

	public bool RepalceMp4;

	public List<Cutscene> OnlineCutscenes = new List<Cutscene>
	{
		new Cutscene("heist_int", ""),
		new Cutscene("apa_fin_cel_apt2", ""),
		new Cutscene("apa_fin_cel_apt3", ""),
		new Cutscene("apa_fin_cel_apt4", ""),
		new Cutscene("apa_hum_fin_int", ""),
		new Cutscene("apa_nar_int", ""),
		new Cutscene("apa_nar_mid", ""),
		new Cutscene("apa_pri_int", ""),
		new Cutscene("MP_INTRO_CONCAT", "GTA Online intro"),
		new Cutscene("mp_intro_concat_seq", "GTA Online intro"),
		new Cutscene("mp_intro_mcs_5_alt1", "GTA Online first race intro male"),
		new Cutscene("mp_intro_mcs_5_alt2", "GTA Online first race intro female"),
		new Cutscene("mp_intro_mcs_8_alt1", "GTA Online first race win"),
		new Cutscene("mph_tut_mid", ""),
		new Cutscene("mph_tut_mcs1", "Pacific Standard Bank - Silent Alarm Press"),
		new Cutscene("mph_tut_int", ""),
		new Cutscene("mph_tut_fin_int", "Player's Meet Lester in their Appartment"),
		new Cutscene("mph_tut_ext", "The Fleeca Heist - Cargobob picks up Kuruma"),
		new Cutscene("mph_tut_car_ext", "The Fleeca Heist - Deliver Kuruma to Lester's Factory"),
		new Cutscene("mph_pri_unf_mcs1", ""),
		new Cutscene("mph_pri_unf_ext", ""),
		new Cutscene("mph_pri_sta_mcs2", "Prison Break - Deliver Casco to Sandy Shores"),
		new Cutscene("mph_pri_sta_mcs1", "Prison Break - Retrieve Casco out of container"),
		new Cutscene("mph_pri_sta_ext", ""),
		new Cutscene("mph_pri_pla_ext", "The Prison Break - Deliver Velum to Los Santos Airport"),
		new Cutscene("mph_pri_mid", ""),
		new Cutscene("mph_pri_int", "Agent 14 Meets Player at their Appartment"),
		new Cutscene("mph_pri_fin_mcs2", "Player's parachute out of plane after rescuing Rashkovsky"),
		new Cutscene("mph_pri_fin_mcs1", "Player's meet Rashkovsky"),
		new Cutscene("mph_pri_fin_int", ""),
		new Cutscene("mph_pri_fin_ext", ""),
		new Cutscene("mph_pri_bus_ext", "Prison Break - Deliver Bus"),
		new Cutscene("mph_pac_wit_mcs1", "Player searches for severs inside Lost MC vans"),
		new Cutscene("mph_pac_pho_ext", ""),
		new Cutscene("mph_pac_mid", ""),
		new Cutscene("mph_pac_int", "Lester Dances with Cane"),
		new Cutscene("mph_pac_hac_mcs1", "Deliver Lost MC Vans"),
		new Cutscene("mph_pac_hac_ext", ""),
		new Cutscene("mph_pac_fin_mcs2", "Pacific Standard - Silent Alarm is pressed"),
		new Cutscene("mph_pac_fin_mcs1", "Pacific Standard - Police Arrive"),
		new Cutscene("mph_pac_fin_mcs0", "Pacific Standard - Lock doors"),
		new Cutscene("mph_pac_fin_int", ""),
		new Cutscene("mph_pac_fin_ext", "Pacific Standard - Escape on boat"),
		new Cutscene("mph_pac_con_ext", "Pacific Standard - Deliver barracks"),
		new Cutscene("mph_pac_bik_ext", "Pacific Standard - Deliver bikes"),
		new Cutscene("mph_nar_wee_ext", "Deliver Weed to Ron"),
		new Cutscene("mph_nar_tra_ext", "Deliver Garbage Truck to Ron"),
		new Cutscene("mph_nar_mid", ""),
		new Cutscene("mph_nar_met_ext", "Deliver tanker to Chef"),
		new Cutscene("mph_nar_int", ""),
		new Cutscene("mph_nar_fin_int", "Ron Waits for Player inside their Appartment"),
		new Cutscene("mph_nar_fin_ext", "Player's deliver Mule to Trevor"),
		new Cutscene("mph_nar_cok_ext", "Deliver coke to Ron"),
		new Cutscene("mph_nar_bik_ext", "Deliver Lost MC van to Ron"),
		new Cutscene("mph_hum_val_ext", "Humane Labs Raid - Deliver Valkyrie"),
		new Cutscene("mph_hum_mid", ""),
		new Cutscene("mph_hum_key_mcs1", "Humane Labs Raid - Access codes"),
		new Cutscene("mph_hum_key_ext", " "),
		new Cutscene("mph_hum_int", "Humane Labs Raid - Agent 14 waits for Player's inside their Appartment"),
		new Cutscene("mph_hum_fin_mcs1", "Humane Labs Raid - Player's retrieve data"),
		new Cutscene("mph_hum_fin_int", ""),
		new Cutscene("mph_hum_fin_ext", "Humane Labs Raid - Player's Arrive with Valkyrie"),
		new Cutscene("mph_hum_emp_ext", "Humane Labs Raid - Player Arrives with Hydra with Emp"),
		new Cutscene("mph_hum_del_ext", "Humane Labs Raid - Player's Escape the Humaine Labs"),
		new Cutscene("mph_hum_arm_ext", "Humane Labs Raid - Player's Deliver Insurgents"),
		new Cutscene("mph_fin_cel_tre", "Trevor bangs head on tree"),
		new Cutscene("mph_fin_cel_str", "Player's Celebrate at Strip club"),
		new Cutscene("mph_fin_cel_roo", "Player's Celebrate overlooking Los Santos"),
		new Cutscene("mph_fin_cel_bar", "Player's Celebrate at Bar"),
		new Cutscene("mph_fin_cel_apt", "Player's Celebrate at Appartment"),
		new Cutscene("low_tra_int", ""),
		new Cutscene("low_tra_ext", ""),
		new Cutscene("low_pho_int", ""),
		new Cutscene("low_pho_ext", ""),
		new Cutscene("low_int", ""),
		new Cutscene("low_fun_mcs1", ""),
		new Cutscene("low_fun_int", ""),
		new Cutscene("low_fun_ext", ""),
		new Cutscene("low_fin_mcs1", ""),
		new Cutscene("low_fin_int", ""),
		new Cutscene("low_fin_ext", ""),
		new Cutscene("low_drv_int", ""),
		new Cutscene("low_drv_ext", ""),
		new Cutscene("impexp_int_l1", "Vehicle Warehouse Intro (Male)"),
		new Cutscene("impexp_int", "Vehicle Warehouse Intro (Female)"),
		new Cutscene("bunk_int", "Agent 14 explains the Gunrunning Bunkers"),
		new Cutscene("hang_int_Plane", "Cuban 800 takes off from Fort Zancudo"),
		new Cutscene("hang_int", "Player enters the hanger to meet Ron"),
		new Cutscene("subj_mcs1", ""),
		new Cutscene("subj_ext", ""),
		new Cutscene("sub_int", ""),
		new Cutscene("silj_mcs2", ""),
		new Cutscene("silj_mcs1", ""),
		new Cutscene("silj_int", ""),
		new Cutscene("silj_ext", ""),
		new Cutscene("sil_pred_mcs1", ""),
		new Cutscene("sil_int", ""),
		new Cutscene("iaaj_int", ""),
		new Cutscene("iaaj_ext", ""),
		new Cutscene("iaa_int", ""),
		new Cutscene("tale_intro", "Pickup Tales of Us"),
		new Cutscene("smun_intro2", "Pickup Solomon from his private plane"),
		new Cutscene("smun_intro", "Solomons Planes goes out of control"),
		new Cutscene("dixn_intro", "Pickup Dixon from hotel"),
		new Cutscene("club_open", "Dave introduces the player to their new nightclub"),
		new Cutscene("club_intro2", "Nightclub Opens"),
		new Cutscene("bmad_intro", "Player Enters Nightclub for the first time"),
		new Cutscene("arena_int", "Player Meets Bryony inside Arena War Workshop"),
		new Cutscene("arena_int2", "Player Intro to Arena War"),
		new Cutscene("mpsui_int", "Player enters Penthouse"),
		new Cutscene("mpcas6_mcs1", "Duggan escapes via helicopter"),
		new Cutscene("mpcas6_int", ""),
		new Cutscene("mpcas6_ext", ""),
		new Cutscene("mpcas5_int", ""),
		new Cutscene("mpcas5_ext", ""),
		new Cutscene("mpcas4_int", ""),
		new Cutscene("mpcas3_mcs1", ""),
		new Cutscene("mpcas3_int", ""),
		new Cutscene("mpcas3_ext", ""),
		new Cutscene("mpcas2_mcs1", ""),
		new Cutscene("mpcas2_int", ""),
		new Cutscene("mpcas2_ext", ""),
		new Cutscene("mpcas1_int", ""),
		new Cutscene("mpcas1_ext", ""),
		new Cutscene("mpcas_int", "Player is welcomed into Casino"),
		new Cutscene("hs3f_sub_vlt", "Approach Vault as Grouppe6 "),
		new Cutscene("hs3f_sub_cel", ""),
		new Cutscene("hs3f_mul_vlt", "Open Vault"),
		new Cutscene("hs3f_mul_rp2", "Rappell down to elevators"),
		new Cutscene("hs3f_mul_rp1", "Rappell down to elevators (2)"),
		new Cutscene("hs3f_int2", "The Diamond Casino Setup Start"),
		new Cutscene("hs3f_int1", "Jimmy Conversation in Arcade"),
		new Cutscene("hs3f_dir_sew", "Player's plant bomb on tunnel"),
		new Cutscene("hs3f_dir_ent", "Player's enter casino - Loud Approach"),
		new Cutscene("hs3f_all_esc_b", "Swat Arrive at Casino"),
		new Cutscene("hs3f_all_esc", "Swat Arrive at Casino (2)"),
		new Cutscene("hs3f_all_drp3", "Deliver goods to buyer"),
		new Cutscene("hs3f_all_drp2", "Deliver goods to vagos"),
		new Cutscene("hs3f_all_drp1", "Deliver goods to cartel"),
		new Cutscene("hs3_prp_vin2", "Vincent - shopping mall"),
		new Cutscene("hs3_prp_vin1", "Vincent - shopping mall (2)"),
		new Cutscene("hs3_prp_celb", ""),
		new Cutscene("hs3_pln_int", "Jimmy tries to play Arcade game"),
		new Cutscene("hs3_int", "Player Meets Lester in Mirror Park"),
		new Cutscene("hs3_ext", "Player Meets Lester on Roof after the Diamond Casino Heist"),
		new Cutscene("hs3_cel_arc", "Player's Celebrate inside Arcade"),
		new Cutscene("hs3_arc_int", "Player enters Arcade"),
		new Cutscene("hs4_vel_lsa_isd", "Velum Approches Cayo Perico"),
		new Cutscene("hs4_sub_int", "Player Enters Kosatka and meets Pavel"),
		new Cutscene("hs4_scp_push", "Player is thrown out of restricted area on Cayo Perico"),
		new Cutscene("hs4_scp_knck", "Player is knocked out on Cayo Perico"),
		new Cutscene("hs4_scp_cctv", "Observe CCTV cameras"),
		new Cutscene("hs4_scp_bch", "Player Arrives at party with Dave"),
		new Cutscene("hs4_nimb_lsa_isd_repeat", "Nimbus Lands on Cayo Perico"),
		new Cutscene("hs4_nimb_lsa_isd", "Dave and Keinemusik prepare to leave LS to fly to Cayo Perico"),
		new Cutscene("hs4_nimb_isd_lsa", "Pilot Leaves Cayo Perico with Nimbus"),
		new Cutscene("hs4_lsa_take_vel", "Velum Leaves LS to fly to Cayo Perico"),
		new Cutscene("hs4_lsa_take_nimb2", "Nimbus Leaves LS to fly to Cayo Perico"),
		new Cutscene("hs4_lsa_land_vel", "Velum lands at Los Santos Airport"),
		new Cutscene("hs4_lsa_land_nimb", "Nimbus Leaves Cayo Perico"),
		new Cutscene("hs4_isd_take_vel", "Velum Leaves Cayo Perico"),
		new Cutscene("hs4_isd_take_nimb2", "Nimbus Leaves Cayo Perico"),
		new Cutscene("hs4_cas_int", "Player Approaches Dave at the Music Locker"),
		new Cutscene("hs4_bch_end", "Party on Cayo Perico start cutscene"),
		new Cutscene("hs4f_tun_ext", "Compound Tunnel Enterance cutscene"),
		new Cutscene("hs4f_tro_ent", "Compound Trogan Enterance"),
		new Cutscene("hs4f_ssg_ext", "Compound Side Door Enterance"),
		new Cutscene("hs4f_ssg_ent", "Compound Side Door Enterance (2)"),
		new Cutscene("hs4f_nrp_ent", "Compound Grapple Enterance"),
		new Cutscene("hs4f_main_ext", "Compound Main Enterance"),
		new Cutscene("hs4f_main_Ent", "Compound Main Enterance (2)"),
		new Cutscene("hs4f_int", "Player's Meet Pavel in Kosatka"),
		new Cutscene("hs4f_ext", "Cayo Perico Heist Ending Celebration"),
		new Cutscene("hs4f_drp_off", "Cayo Perico Heist drop bag to buyer"),
		new Cutscene("hs4f_drp_cel", "Cayo Perico Heist Ending Celebration (2)"),
		new Cutscene("hs4f_app_sub", "Cayo Perico Heist Approach Kosatka"),
		new Cutscene("hs4f_app_plne", "Cayo Perico Heist Approach Alkonst"),
		new Cutscene("hs4f_app_heli", "Cayo Perico Heist Approach Annihilator Stealth"),
		new Cutscene("tunf_uni_vlt", "Open Vault"),
		new Cutscene("tunf_uni_swat", "Swat Arrives"),
		new Cutscene("tunf_iaa_mcs1", ""),
		new Cutscene("tunf_drp_hand", "Player's Meets Chef"),
		new Cutscene("tunf_drp_bag", "Player's drop off bag"),
		new Cutscene("tunf_drp_avi", ""),
		new Cutscene("tun_shop_int", "Player's Enter tuner shop"),
		new Cutscene("tun_shop_ext", "Player's Deliver car to Tuner Shop"),
		new Cutscene("tun_meet_int", "Car Meet Intro"),
		new Cutscene("tun_asd_ext", "Deliver car to Car Meet"),
		new Cutscene("fix_wrm_int", "Player Visits Franklin and Lamar in Record A Studios"),
		new Cutscene("fix_weed_int", "Player Visits Franklin and Lamar in Record A Studios (2)"),
		new Cutscene("fix_weed_ext", "Player Leaves Franklin and Lamar in Record A Studios"),
		new Cutscene("fix_trk_mcs2", "The Contract - Put Lawyer in boot"),
		new Cutscene("fix_trip3_mcs1", "Short Trip Mission 3 Cutscene "),
		new Cutscene("fix_trip3_int", "Short Trip Mission 3 Start Cutscene"),
		new Cutscene("fix_trip3_ext", "Short Trip Mission 3 End Cutscene"),
		new Cutscene("fix_trip2_mcs1", "Short Trip Mission 2 Cutscene"),
		new Cutscene("fix_trip2_int", "Short Trip Mission 2 Start Cutscene"),
		new Cutscene("fix_trip2_ext", "Short Trip Mission 2 End Cutscene"),
		new Cutscene("fix_trip1_mcs3", "Short Trip Mission 1 Franklin and Lamar jump out of mule"),
		new Cutscene("fix_trip1_mcs2", "Short Trip Mission 1 Franklin and Lamar hitch ride inside mule"),
		new Cutscene("fix_trip1_mcs1", "Short Trip Mission 1 Chop Attack ped"),
		new Cutscene("fix_trip1_int", "Short Trip Mission 1 Start Mission "),
		new Cutscene("fix_trip1_ext", "Short Trip Mission 1 End Mission"),
		new Cutscene("fix_stu_mcs1", "Dr Dre Record at Record A Studios"),
		new Cutscene("fix_stu_ext", "Deliver to Record A Studios"),
		new Cutscene("fix_set_mcs1", "The Contract Setup 1 Enter FIB Building"),
		new Cutscene("fix_prod_int", "Dre Dre Enters Records A Studios"),
		new Cutscene("fix_prod_ext", "Dre Dre Records Song at Record A Studios"),
		new Cutscene("fix_pro_mcs1", "The Contract - Steal Music Equipment"),
		new Cutscene("fix_low_mcs2", "The Contract - Vagos Escapes with Lowrider"),
		new Cutscene("fix_low_mcs1", "The Contract - Families and Ballas vs Vagos"),
		new Cutscene("fix_golf_int", "Meet Franklin at Golf Club"),
		new Cutscene("fix_golf_ext", "Franklin calls Dr Dre at Peir"),
		new Cutscene("fix_bil_mcs2", "Billionares Helicopter crashes"),
		new Cutscene("fix_bil_mcs1", "Billionare Escapes Mansion"),
		new Cutscene("fix_art_int", "Recording at Record A Studios"),
		new Cutscene("fix_art_ext", "Recording at Record A Studios "),
		new Cutscene("fix_agy_int3", "Agency Enter 3"),
		new Cutscene("fix_agy_int2", "Agency Enter 2"),
		new Cutscene("fix_agy_int1", "Agency Enter 1"),
		new Cutscene("fixf_fin_mcs3", "The Contract - Ending, Dr Dre Leaves"),
		new Cutscene("fixf_fin_mcs2", "The Contract - Ending, grab Johnny Gunz"),
		new Cutscene("fixf_fin_mcs1", "The Contract - Ending, Johnny Guns Escapes"),
		new Cutscene("xm3_drg1_ace_int", "Drug Wars - Crew enters Ace Liquor "),
		new Cutscene("xm3_drg1_bmx_int", "Drug Wars - Dax Arrives on motorcycle"),
		new Cutscene("xm3_drg1_trn_mcs1", "Drug Wars - Player Switches track for train"),
		new Cutscene("xm3_drg2_cook_int", "Drug Wars - ??"),
		new Cutscene("xm3_drg2_cook_mcs1", "Drug Wars - rescue hostage"),
		new Cutscene("xm3_drg2_pln_ext", "Drug Wars - Dax and player exit Mule"),
		new Cutscene("xm3_drg2_pln_mcs1", "Drug Wars - Velum crashes into cargoplane "),
		new Cutscene("xm3_drg2_pln_mcs2", "Drug Wars - Player takes control of cargoplane"),
		new Cutscene("xm3_drg2_pln_mcs3", "Drug Wars - cargo plane lands at Trevor's Airfield"),
		new Cutscene("xm3_drg2_rhb_mcs1", "Drug Wars - Player tries to open door"),
		new Cutscene("sum23_cm1_int", "San Andreas Mercanaries - Project Overthrow Start"),
		new Cutscene("sum23_cm6_ext", "San Andreas Mercanaries - Project Overthrow Ending"),
		new Cutscene("xm4_cbr_fin_ext", "The Chop Shop"),
		new Cutscene("xm4_cbr_fin_ext2", "The Chop Shop"),
		new Cutscene("xm4_cbr_vin_int", "The Chop Shop"),
		new Cutscene("xm4_cbr_vin_int2", "The Chop Shop"),
		new Cutscene("xm4_cbr6_agg_int", "The Chop Shop"),
		new Cutscene("xm4_cbr6_slth_int", "The Chop Shop"),
		new Cutscene("xm4_rob_cas_mcs1", "The Chop Shop"),
		new Cutscene("xm4_rob_cas_mcs2", "The Chop Shop"),
		new Cutscene("xm4_rob_ship_mcs1", "The Chop Shop"),
		new Cutscene("xm4_rob_sub_mcs1", "The Chop Shop"),
		new Cutscene("xm4_rr_fin_mcs1", "The Chop Shop"),
		new Cutscene("xm4_rr_fin_mcs2", "The Chop Shop"),
		new Cutscene("xm4_yard_ext", "The Chop Shop"),
		new Cutscene("xm4_yard_int", "The Chop Shop"),
		new Cutscene("sum24_btw_cell_ext", "Bottom Dollar Bounties - Lock Target in Sell"),
		new Cutscene("sum24_btw_arrdead_mcs1", "Bottom Dollar Bounties - Pickup Target, Put in Back of Van"),
		new Cutscene("sum24_btw_arralive_mcs6", "Bottom Dollar Bounties - Arrest Target, Put in Back of Van"),
		new Cutscene("sum24_btw_dead_ext", "Bottom Dollar Bounties - Deliver Dead Target"),
		new Cutscene("sum24_bty3_ko_mcs2", "Bottom Dollar Bounties - Player Knocked out"),
		new Cutscene("sum24_bty4_heavy_mcs3", "Bottom Dollar Bounties - Juggernaut Arrives"),
		new Cutscene("sum24_bty6_escape_mcs4", "Bottom Dollar Bounties - Chase Target"),
		new Cutscene("sum24_office_int", "Bottom Dollar Bounties - Bail Bonds Intro"),
		new Cutscene("xm24_rob_crash_mcs1", "Agents of Sabotage - Crash Plane in Alamo"),
		new Cutscene("xm24_rob_crash_ext", "Agents of Sabotage - Deliver bag to client"),
		new Cutscene("xm24_mfh6_fin_ext", "Agents of Sabotage - Land Titan 250D at Airstrip"),
		new Cutscene("xm24_mfh6_plne_dmg_mcs1", "Agents of Sabotage - Titan 250D take damage"),
		new Cutscene("xm24_mfh6_gun_drp_mcs2", "Agents of Sabotage - DH-7 Iron Mule deliver cargo crate"),
		new Cutscene("xm24_mfh1_ext", "Agents of Sabotage - Titan 250D destroy dock boats"),
		new Cutscene("xm24_mfh_int", "Agents of Sabotage - Oscar Guzman Intro"),
		new Cutscene("xm24_factory_int", "Agents of Sabotage - The FIB Files Intro"),
		new Cutscene("sum25_bt2_carthief_mcs1", "Money Fronts - The FIB Files Intro"),
		new Cutscene("sum25_bt2_delivery_ext", "Money Fronts - The FIB Files Intro"),
		new Cutscene("sum25_cayo_surv_ext", "Money Fronts - The FIB Files Intro"),
		new Cutscene("sum25_lsia_int", "Money Fronts - The FIB Files Intro"),
		new Cutscene("xm25_avi_int", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi5_ext", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi5_heli_mcs1", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_exta", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_extb", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_extc", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_rescue_mcs1a", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_rescue_mcs1b", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_rescue_mcs1c", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_shutters_mcs2a", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_shutters_mcs2b", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_avi6_shutters_mcs2c", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_mansion_inta", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_mansion_intb", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("xm25_mansion_intc", "A Safehouse On The Hills- The FIB Files Intro"),
		new Cutscene("sum26_heist_int", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_exta", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_extb", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_cel", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_esc_mcs3", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_esc_mcs3", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_las_mcs2", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_rpl_mcs1a", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_swr_mcs1c", "The Kortz Center Heist - The FIB Files Intro"),
		new Cutscene("sum26_kortz_van_mcs1b", "The Kortz Center Heist - The FIB Files Intro")
	};

	public List<Cutscene> StoryCutscenes = new List<Cutscene>
	{
		new Cutscene("exile_1_int_seq", "Minor Turbulence intro"),
		new Cutscene("exl_1_mcs_1_p3_b", "Minor Turbulence plane crash"),
		new Cutscene("exile_2_int_seq", "Predator intro"),
		new Cutscene("exl_2_mcs_3_seq", "Predator outro"),
		new Cutscene("exile_3_int_seq", "Derailed intro"),
		new Cutscene("exl_3_ext", "Derailed outro"),
		new Cutscene("family_1_int_seq", "Father / Son intro"),
		new Cutscene("fam_1_ext_alt2", "Father / Son lost boat1"),
		new Cutscene("fam_1_ext_alt3", "Father / Son lost boat2"),
		new Cutscene("fam_1_mcs_2", "Father / Son LS customs"),
		new Cutscene("fam_1_ext_2_seq", "Father / Son outro"),
		new Cutscene("family_2_int_seq", "Daddy's Little Girl intro"),
		new Cutscene("family_2_mcs_2_seq", "Daddy's Little Girl bike race win"),
		new Cutscene("family_2_mcs_3_seq", "Daddy's Little Girl yacht confrontation"),
		new Cutscene("family_2_mcs_4_seq", "Daddy's Little Girl outro"),
		new Cutscene("family_3_int_seq", "Marriage Counseling intro"),
		new Cutscene("fam_3_mcs_1_seq", "Marriage Counseling pre house pull down"),
		new Cutscene("family_3_ext_seq", "Marriage Counseling outro"),
		new Cutscene("family_4_int_alt1_seq", "ame or Shame intro"),
		new Cutscene("family_4_mcs_2_seq", "Fame or Shame on set confrontation"),
		new Cutscene("family_4_mcs_3_seq", "Fame or Shame outro"),
		new Cutscene("family_5_int_seq", "Did Somebody Say Yoga ? intro"),
		new Cutscene("family_5_mcs_1", "Did Somebody Say Yoga ? yoga2"),
		new Cutscene("family_5_mcs_2", "Did Somebody Say Yoga ? yoga3"),
		new Cutscene("family_5_mcs_3_seq", "Did Somebody Say Yoga ? yoga finished"),
		new Cutscene("family_5_mcs_4", "Did Somebody Say Yoga ? Jimmy's room"),
		new Cutscene("family_5_mcs_5", " Did Somebody Say Yoga ? Burger Shot"),
		new Cutscene("family_5_mcs_6_seq", "Did Somebody Say Yoga ? hallucinations"),
		new Cutscene("family_5_mcs_5_p4", "Did Somebody Say Yoga ? outro"),
		new Cutscene("family_5_mcs_5_p5", "Did Somebody Say Yoga ? post outro"),
		new Cutscene("mic_1_int_seq", "Bury The Hatchet intro"),
		new Cutscene("mic_1_mcs_1_seq", "Bury The Hatchet plane boarding"),
		new Cutscene("mic_1_mcs_2_seq", "Bury The Hatchet grave confrontation"),
		new Cutscene("mic_1_mcs_3", "Bury The Hatchet Michael captured"),
		new Cutscene("mic_2_int_seq", "Fresh Meat intro"),
		new Cutscene("mic_2_mcs_1", "Fresh Meat Michael captured"),
		new Cutscene("mic_2_mcs_3_seq", "Fresh Meat outro"),
		new Cutscene("fam_6_int_seq", "Reuniting The Family intro"),
		new Cutscene("fam_6_int_p3_t7", "Reuniting The Family Jimmy"),
		new Cutscene("fam_6_mcs_1", "Reuniting The Family Amanda"),
		new Cutscene("fam_6_mcs_2_seq", "Reuniting The Family Tracey"),
		new Cutscene("fam_6_mcs_3", "Reuniting The Family post piercings"),
		new Cutscene("fam_6_mcs_4_seq ", "Reuniting The Family post tattoo"),
		new Cutscene("fam_6_mcs_5", "Reuniting The Family post haircut"),
		new Cutscene("fam_6_mcs_6_seq", "Reuniting The Family therapy session"),
		new Cutscene("fam_6_mcs_6_p4_b", "euniting The Family outro"),
		new Cutscene("mic_4_int_seq", "Meltdown intro"),
		new Cutscene("abigail_mcs_1_concat", "Michael Meet Abigail"),
		new Cutscene("abigail_mcs_2", "Michael complete Abigail Mission"),
		new Cutscene("ah_1_ext_t6", "Michael complains about Steve Haines in Lesters factory"),
		new Cutscene("ah_1_int", "Start of FIB Heist"),
		new Cutscene("ah_1_mcs1", "Michael talks to Harvey (Union DP Janitor)"),
		new Cutscene("ah_2_ext_alt", "Franklin Deliver FIB Floor Plans"),
		new Cutscene("ah_2_ext_p4", "Choose Crew for FIB Heist cutscene"),
		new Cutscene("ah_3a_ext", "Michael and Franklin Visit Lester after FIB Heist"),
		new Cutscene("ah_3a_int", "Lester Close down Factory before FIB Heist"),
		new Cutscene("ah_3a_mcs_3", "Crew move through FIB Building staircase"),
		new Cutscene("ah_3a_mcs_6", "FIB Building Rappel in Elevator"),
		new Cutscene("ah_3b_ext", "FIB crew celebrate at Franklins house"),
		new Cutscene("ah_3b_int", "??"),
		new Cutscene("ah_3b_mcs_1", "FIB Heist Rappel from heli"),
		new Cutscene("ah_3b_mcs_2", "FIB Heist cut glass hatch"),
		new Cutscene("ah_3b_mcs_3", "??"),
		new Cutscene("ah_3b_mcs_4", "??"),
		new Cutscene("ah_3b_mcs_5", "??"),
		new Cutscene("ah_3b_mcs_7", "??"),
		new Cutscene("armenian_1_mcs_1", "Jimmy,Simeon, Lamar and Franklin talk inside Simeon's Dealership"),
		new Cutscene("armenian_2_int", "Simeon hugs Franklin in Simeon's Dealership"),
		new Cutscene("armenian_2_mcs_6", "Lamar and Franklin Argue at car wash"),
		new Cutscene("armenian_3_int", "Simeon Argues with driver of yellow vehicle until Franklin arives"),
		new Cutscene("armenian_3_mcs_3", "Jimmy Freaks out that Franklin is in the house"),
		new Cutscene("armenian_3_mcs_4", "Tracey Freaks out that Franklin is in the house"),
		new Cutscene("armenian_3_mcs_5", "Amanda & Coach Freaks out that Franklin is in the house"),
		new Cutscene("armenian_3_mcs_6", "Michael orders Franklin to drive through Simeons Dealership window"),
		new Cutscene("armenian_3_mcs_8", "Simeon reacts to Franklin driving through Simeons Dealership window"),
		new Cutscene("armenian_3_mcs_9_concat", "Michael and Simeon Fight"),
		new Cutscene("arm_1_mcs_2_concat", "Lamar roasts Franklin"),
		new Cutscene("arm_2_mcs_4", "Franklin and Lamar look for bike in garage - ambush by Vagos"),
		new Cutscene("ass_int_2_alt1", "Franklin meets lester on vespucci beach seat"),
		new Cutscene("ass_mcs_1", "Assassination on bus, man steals bike from lady"),
		new Cutscene("bar_1_rcm_p2", "Michael meets Barry"),
		new Cutscene("bar_2_rcm", "Michael meets Barry after mission"),
		new Cutscene("bar_3_rcm", "Trevor meets Barry"),
		new Cutscene("bar_4_rcm", "Trevor meets Barry after mission"),
		new Cutscene("bar_5_rcm_p2", "Franklin Meets Barry"),
		new Cutscene("bss_1_mcs_2", "Michael and Franklin investigate Union Depository"),
		new Cutscene("bss_1_mcs_3", "stockades arive at Union Depository"),
		new Cutscene("bs_1_int", "??"),
		new Cutscene("bs_2a_2b_int", "Crew arives at Stripclub before Union Depository Heist"),
		new Cutscene("bs_2a_ext", "Crew meets at Michaels house after Union Depository Heist"),
		new Cutscene("bs_2a_int", "Crew arives at Stripclub before Union Depository Heist"),
		new Cutscene("bs_2a_mcs_1", "Crew stickup Stockades and steals them in Union Depository Heist"),
		new Cutscene("bs_2a_mcs_10", "??"),
		new Cutscene("bs_2a_mcs_11", "Michael,Trevor and Franklin head back to LS after securing gold in trailers"),
		new Cutscene("bs_2a_mcs_2", "??"),
		new Cutscene("bs_2a_mcs_3_alt", "Union DP Elevator Scene"),
		new Cutscene("bs_2a_mcs_8_p3", "Crew leaves in Gauntlets during Union Depository Heist"),
		new Cutscene("bs_2b_ext_alt1a", "Crew arives at Trevor's Airfield (Grapeseed) after Union Depository Heist, Good ending"),
		new Cutscene("bs_2b_ext_alt2", "Crew arives at Trevor's Airfield (Grapeseed) after Union Depository Heist, Bad ending "),
		new Cutscene("bs_2b_int", "Crew arives at Stripclub before Union Depository Heist"),
		new Cutscene("bs_2b_mcs_1", "Helicopter Pickup Gold"),
		new Cutscene("bs_2b_mcs_3", "??"),
		new Cutscene("car_1_ext_concat", "Michael, Trevor and Franklin meet Devin Weston at his workshop"),
		new Cutscene("car_1_int_concat", "Michael, Trevor and Franklin meet Devin Weston at Construction site"),
		new Cutscene("car_2_mcs_1", "Franklin deliver Z Type"),
		new Cutscene("car_4_ext", "Franklin deliver JB700"),
		new Cutscene("car_4_mcs_1", "Franklin deliver hide male Actor and steal clothes"),
		new Cutscene("car_5_ext", "Franklin Meets with Molly"),
		new Cutscene("car_5_mcs_1", "Take Vehicles to Paleto Bay Start mission"),
		new Cutscene("car_steal_3_mcs_1", "Franklin Races two people at gas station"),
		new Cutscene("car_steal_3_mcs_2", "Michael and Trevor prepare to chase Franklin and two other races on police bikes"),
		new Cutscene("car_steal_3_mcs_3", "Michael and Trevor pull over Franklin and two other races on police bikes and steal cars"),
		new Cutscene("chinese_1_int", "Trevor goes to Yellowjack told he is still banned"),
		new Cutscene("chinese_2_int", "Trevor meets with Tao Cheng and his translator at Yellowjack"),
		new Cutscene("chi_1_mcs_1", "Trevor kicks Tao Check and Translator into ice box"),
		new Cutscene("chi_1_mcs_4_concat", "Trevor unlocks ice box for Tao Cheng and his Translator"),
		new Cutscene("chi_2_mcs_5", "Oneils talk at farm"),
		new Cutscene("choice_int", "Devin Weston asks Franklin to kill Michael (The Third Way)"),
		new Cutscene("cletus_mcs_1_concat", "Trevor goes hunting with Cletus"),
		new Cutscene("dhp1_mcs_1", "Trevor and Floyd steal submarine"),
		new Cutscene("drf_mic_1_cs_1", "Michael sees Dr Friedlander"),
		new Cutscene("drf_mic_1_cs_2", "Michael sees Dr Friedlander"),
		new Cutscene("drf_mic_2_cs_1", "Michael sees Dr Friedlander"),
		new Cutscene("drf_mic_2_cs_2", "Michael sees Dr Friedlander"),
		new Cutscene("drf_mic_3_cs_1", "Michael sees Dr Friedlander"),
		new Cutscene("drf_mic_3_cs_2", "Michael sees Dr Friedlander"),
		new Cutscene("ef_1_rcm", "Michael Meets Mary-Ann Quinn"),
		new Cutscene("ef_2_rcm", "Trevor Meets Mary-Ann Quinn"),
		new Cutscene("ef_3_rcm_concat", "Franklin Meets Mary-Ann Quinn"),
		new Cutscene("es_1_rcm_concat", "Franklin helps Dom down from Tree"),
		new Cutscene("es_1_rcm_p1", "Franklin Meets dog (Dom Mission)"),
		new Cutscene("es_2_rcm_concat", "Franklin Meets Dom at LSIA"),
		new Cutscene("es_3_mcs_1", "Franklin and Dom conversation at repair place"),
		new Cutscene("es_3_mcs_2", "Franklin and Dom Argue at repair place"),
		new Cutscene("es_3_rcm", "Franklin and Dom jump off maze bank"),
		new Cutscene("es_4_rcm_p1", "Franklin meet dog (Dom Mission)"),
		new Cutscene("es_4_rcm_p2_concat", "Franklin watch Dom jump off dam and die"),
		new Cutscene("exile_1_int", "Michael Calls Franklin in Trevors Trailer after kidnapping Patricia"),
		new Cutscene("exile_2_int", "Michael Calls Jimmy in Trevors Trailer after kidnapping Patricia"),
		new Cutscene("exile_3_int", "Michael talks to Nervous Ron in Trevors Trailer after kidnapping Patricia"),
		new Cutscene("exl_1_mcs_1_p3_b", "Trevor crashes Cuban-800 into Cargoplane"),
		new Cutscene("exl_2_mcs_3", "Michael,Trevor and Franklin Exit heli and head to SUV"),
		new Cutscene("exl_3_ext", "Michael and Trevor look at what the take from the train is"),
		new Cutscene("family_1_int", "Tracy Arguing with Amanda"),
		new Cutscene("family_2_int", "Michael Smash Jimmy's TV"),
		new Cutscene("family_2_mcs_2", "Cutscene after Michael And Jimmy race on bikes to the peir"),
		new Cutscene("family_2_mcs_3", "Michael rescue Tracy from porn dudes on yacht"),
		new Cutscene("family_2_mcs_4", "Tracy attacks Jimmy"),
		new Cutscene("family_3_ext", "Martin Madrazo beats up Michael, after he pulls his house down"),
		new Cutscene("family_3_int", "??"),
		new Cutscene("family_3_intp1", ""),
		new Cutscene("family_3_intp1_2", ""),
		new Cutscene("family_3_intp1_3", ""),
		new Cutscene("family_3_intp2", ""),
		new Cutscene("family_3_intp2_1", ""),
		new Cutscene("family_3_mcs_1", ""),
		new Cutscene("family_4_mcs_2", "Michael and Trevor pickup Tracy from Fame or Shame"),
		new Cutscene("family_4_mcs_3_concat", "Michael and Trevor force Lazlow to dance "),
		new Cutscene("family_5_int", "Michael and Amanda Argue then Fabien comes in (Sombody Say Yoga)"),
		new Cutscene("family_5_mcs_1", "Michael does Yoga (Sombody Say Yoga)"),
		new Cutscene("family_5_mcs_2", "Michael does Yoga 2 (Sombody Say Yoga)"),
		new Cutscene("family_5_mcs_3", "Michael falls into pool (Sombody Say Yoga)"),
		new Cutscene("family_5_mcs_4", "Michael and Jimmy go for drive "),
		new Cutscene("family_5_mcs_5", "Jimmy buys drugs at Burger Shot"),
		new Cutscene("family_5_mcs_5_p4", "Michael reads note from Amanda after she, Jimmy and Tracey leave"),
		new Cutscene("family_5_mcs_5_p5", "Michael sits on front porch after Amanda, Jimmy and Tracey leave"),
		new Cutscene("fam_1_ext_2", "Franklin drops Jimmy off at his house after Father/Son Mission"),
		new Cutscene("fam_1_ext_alt2", "Michael argues with Jimmy, fail Father/Son Mission"),
		new Cutscene("fam_1_ext_alt3", "Michael Swares in his care,fail Father/Son Mission "),
		new Cutscene("fam_1_mcs_2", "Michael exits Amanda's car at LS Customs and takes taxi (Father/Son)"),
		new Cutscene("fam_3_mcs_1", "Michael and Franklin pull house off cliff"),
		new Cutscene("fam_4_int_alt1", "Trevor arrives at Michaels House ("),
		new Cutscene("fam_5_mcs_6", "UFO suck up Michael Scene (Fame Or Shame)"),
		new Cutscene("fam_5_mcs_6_p3_b", ""),
		new Cutscene("fam_5_mcs_6_p4", ""),
		new Cutscene("fam_6_int", "Michael Watch TV"),
		new Cutscene("fam_6_int_p3_t7", "Jimmy Arives home to find Michael asleep on the couch"),
		new Cutscene("fam_6_mcs_1", "Michael meets Amanda and Fabien at coffee place"),
		new Cutscene("fam_6_mcs_2_concat", "Tracy Talks with Lazlow in Tattoo Palor"),
		new Cutscene("fam_6_mcs_3", "Michael rips Lazlows shirt off in Tattoo Palor"),
		new Cutscene("fam_6_mcs_4", "Michael Tattoo Lazlows Chest"),
		new Cutscene("fam_6_mcs_5", "Michael cuts off Lazlow's Pony tail"),
		new Cutscene("fam_6_mcs_6", "Family meeting at Dr Friedlander's"),
		new Cutscene("fam_6_mcs_6_p4_b", "Amanda, Tracey and Jimmy arive home with Michael after leaving"),
		new Cutscene("fbi_1_ext", "Michael warns Franklin about Trevor and the Deal with the FIB"),
		new Cutscene("fbi_1_int", "Michael meets Dave Norton at Observatory"),
		new Cutscene("fbi_1_mcs_1_concat", ""),
		new Cutscene("fbi_2_ext", "Deliver Mr K by heli (Three's Company)"),
		new Cutscene("fbi_2_int", "Michael Meets Steve Haines (Three's Company)"),
		new Cutscene("fbi_2_mcs_1", "Trevor Arrives at garbage lot (Three's Company)"),
		new Cutscene("fbi_2_mcs_2", "Michael Rappels from heli (Three's Company)"),
		new Cutscene("fbi_2_mcs_3b", "Leaving with Mr K (Three's Company)"),
		new Cutscene("fbi_3_int", "Trevor Shits down at docks before torturing Mr K (By The Book)"),
		new Cutscene("fbi_3_mcs_1", "Torture Cutscene 1 (By The Book)"),
		new Cutscene("fbi_3_mcs_2", "Michael and Dave Scope out target (By The Book)"),
		new Cutscene("fbi_3_mcs_3", "Steve Haines interogates Mr K (By The Book)"),
		new Cutscene("fbi_3_mcs_4p2", "Torture Cutscene 2 (By The Book)"),
		new Cutscene("fbi_3_mcs_5", "Steve Haines interogates Mr K 2 (By The Book)"),
		new Cutscene("fbi_3_mcs_5p2", "Steve Haines interogates Mr K 3 (By The Book)"),
		new Cutscene("fbi_3_mcs_6p1_b", "Steve Haines and Trevor argue after Mr K dies (By The Book)"),
		new Cutscene("fbi_3_mcs_6p2", "Steve Haines interogates Mr K 3 (By The Book)"),
		new Cutscene("fbi_3_mcs_7", "End Torture scene (By The Book)"),
		new Cutscene("fbi_3_mcs_8", "Trevor drops Mr K off at the airport"),
		new Cutscene("fbi_4_int", "??"),
		new Cutscene("fbi_4_mcs_2_concat", "Michael delivers package to Devin Weston"),
		new Cutscene("fbi_4_mcs_3_concat", "Hand Out masks at garbage lot (The Merryweather Heist)"),
		new Cutscene("fbi_5_int", "Start Humaine Labs Heist cutscene "),
		new Cutscene("fbi_5a_mcs_1", "Michael, Dave Norton and Steve Haines get into the Humaine Labs (Humane Labs Heist)"),
		new Cutscene("fbi_5a_mcs_10", "??"),
		new Cutscene("fbi_5b_mcs_1", "Steve Haines shoots himself in the knee (Humane Labs Heist)"),
		new Cutscene("fbi_5_ext", "Trevor drops off Patricia at Martin Madrazo's House"),
		new Cutscene("fin_a_ext", "Kill Trevor end Cutscene (Somthing Sensible)"),
		new Cutscene("fin_a_int", " Kill Trevor start Cutscene (Somthing Sensible)"),
		new Cutscene("fin_b_ext", "Kill Michael end Cutscene (The Time's Come)"),
		new Cutscene("fin_b_mcs_1_aandb", "Kill Michael start Cutscene (The Time's Come)"),
		new Cutscene("fin_b_mcs_2", "Kill Michael middle Cutscene (The Time's Come)"),
		new Cutscene("fin_c2_mcs_1", "Kill Steve Haines (The Third Way)"),
		new Cutscene("fin_c2_mcs_5", "Trevor Kidnaps Devin Weston (The Third Way)"),
		new Cutscene("fin_c_ext", "Michael, Trevor and Franklin choose Option C (The Third Way)"),
		new Cutscene("fin_c_int", "Franklin talks about his options (The Third Way)"),
		new Cutscene("fin_c_mcs_1", "Franklin talks to Michael and Trevor at the Foundry (The Third Way)"),
		new Cutscene("fin_c_mcs_1_p1_a", "Franklin talks to Michael and Trevor at the Foundry (The Third Way)"),
		new Cutscene("fin_ext_p1", "GTAV Ending Scene 1 (The Third Way)"),
		new Cutscene("fin_ext_p2", "Michael, Frankling and Trevor push Devin Weston's car off cliff"),
		new Cutscene("fra_0_int", "Franklin returns to his aunts house and hides from his aunt and her friend (Chop)"),
		new Cutscene("fra_0_int_p1_alt", "Franklin returns to his aunts house and hides from his aunt and her friend (Chop)"),
		new Cutscene("fra_0_mcs_1", "Franklin and Lamar harras biker (Chop)"),
		new Cutscene("fra_0_mcs_4_p2_t3", "Chop Attack Biker (Chop)"),
		new Cutscene("fra_0_mcs_5_p1", "Franklin, Lamar and Chop arrive at High School in van (Chop)"),
		new Cutscene("fra_1_mcs_1", "Franklin, Lamar and Trevor get scammed at Grove St (Hood Safari)"),
		new Cutscene("fra_2_ext", "Lamar and Franklin talk (Hood Safari)"),
		new Cutscene("fra_2_ig_4_alt1_concat", "Michael, Trvor and Franklin rescue Lamar (Lamar Down)"),
		new Cutscene("fra_2_int", "Tanisha arrives at Franklins House (Hood Safari) "),
		new Cutscene("hao_mcs_1", "Franklin meet Hao"),
		new Cutscene("jh2_fina_mcs4_a1a2", "Micheal throws police man onto the ground (Jewel Store Job)"),
		new Cutscene("jh_1_ig_3", "Michael asks about Jewelery (Jewe Store Job Setup)"),
		new Cutscene("jh_1_int", "Michael and Lester talk about the Jewel Store heist"),
		new Cutscene("jh_1_mcs_4p2", "Michael and Lester talk about the Jewel Store heist ending scene"),
		new Cutscene("jh_1_mcs_4_p1_alt1", "Michael and Lester talk about the Jewel Store heist ending scene alt"),
		new Cutscene("jh_2a_intp4", "Michael runs through the Jewel Store job with the Crew (Jewel Store Job)"),
		new Cutscene("jh_2a_mcs_1", "Gas knocks out everyone in Jewel Store (Jewel Store Job)"),
		new Cutscene("jh_2b_int", "Michael runs through the Jewel Store job with the Crew (Jewel Store Job)"),
		new Cutscene("jh_2b_mcs_1", "Crew Enter Jewel Store, loud (Jewel Store Job)"),
		new Cutscene("jh_2_arrest_fail_c", "Crew Arrested Fail Jewel Store Job (Jewel Store Job)"),
		new Cutscene("jh_2_celeb", "Jewel Store Job Complete Celebration (Jewel Store Job)"),
		new Cutscene("jh_2_fin_a_mcs4_a1", "Micheal throws police man onto the ground (Jewel Store Job) "),
		new Cutscene("jh_end_pt2_a1_p2_a", "??"),
		new Cutscene("josh_1_int", "Josh Mission Cutscene"),
		new Cutscene("josh_1_int_concat", "Josh Mission Cutscene"),
		new Cutscene("josh_2_intp1_t4", "Josh Mission Cutscene"),
		new Cutscene("josh_3_intp1", "Josh Mission Cutscene"),
		new Cutscene("josh_4_int_concat", ""),
		new Cutscene("lamar_1_int", "Franklin inturupts Aunt Denise and her friends (The Long Stretch)"),
		new Cutscene("lam_1_mcs_1_concat", "Ambush At Scrap Yard (The Long Stretch)"),
		new Cutscene("lam_1_mcs_2", "Lamar, Stretch and Franklin walk to van (The Long Stretch)"),
		new Cutscene("lam_1_mcs_3", "Franklin, Lamar, and Stretch visit Ammunation (The Long Stretch)"),
		new Cutscene("lester_1_int", "Michael Meets Lester (Friend Request)"),
		new Cutscene("les_1a_mcs_0", "Michael Shop for Clothes (Friend Request)"),
		new Cutscene("les_1a_mcs_1", "Michael Talks with Rickie at Life Invader  (Friend Request)"),
		new Cutscene("les_1a_mcs_2", "Michael Sits at Desk  (Friend Request)"),
		new Cutscene("les_1a_mcs_3", "Michael Talks to Rickie about Jay Noris keynote  (Friend Request)"),
		new Cutscene("les_1a_mcs_4", "Michael Plants bomb in keynote phone  (Friend Request)"),
		new Cutscene("les_1b_mcs_1", "Michael Watches keynote  (Friend Request)"),
		new Cutscene("lsdhs_mcs_2", "Floyd gets beat up at Merryweather docks (Scouting the Port)"),
		new Cutscene("lsdhs_mcs_3_p1_concat", "Wade on couch covered in sewage  (Scouting the Port)"),
		new Cutscene("lsdhs_mcs_3_p2", "Floyd and trevor argue about going to the Union (Scouting the Port)"),
		new Cutscene("lsdh_2a_ext", "The Merryweather Heist ending, crane winches up sub and nuke (The Merryweather Heist)"),
		new Cutscene("lsdh_2a_int", "??"),
		new Cutscene("lsdh_2b_int", "??"),
		new Cutscene("lsdh_2b_mcs_1", "The Merryweather Heist ending, Lester shouts at Trevor for stealing a Nuke (The Merryweather Heist)"),
		new Cutscene("lsdh_int", "Trevor and Floyd Argue, Trevor pulls down his pants (Scouting the Port)"),
		new Cutscene("martin_1_ext", "Trevor pulls up to Michael with kidnapped Patricia in the back (Caide Libre)"),
		new Cutscene("martin_1_int", "Trevor pulls up to Michael at Martin Madrazo's House (Caide Libre)"),
		new Cutscene("martin_1_mcs_1", "Trevor steals breifcase from crashed airplane (Minor Turbulence)"),
		new Cutscene("mic_1_int", "Michael and Trevor talk about Brad then Trevor goes to North Yankton (Bury The Hatchet)"),
		new Cutscene("mic_1_mcs_1", "Michael arives at airport to go to North Yankton (Bury The Hatchet)"),
		new Cutscene("mic_1_mcs_2", "Trevor digs up Brad's grave (Bury The Hatchet)"),
		new Cutscene("mic_1_mcs_3", "Cheng's capture Michael (Bury The Hatchet)"),
		new Cutscene("mic_2_int", "Trevor tells Franklins Aunt an her friends to Shut the F up (Fresh Meat)"),
		new Cutscene("mic_2_mcs_1", "Cheng's Interogate Michael at Meat Factory (Fresh Meat)"),
		new Cutscene("mic_2_mcs_3_concat", "Michael thanks Franklin for saving him outside his house (Fresh Meat)"),
		new Cutscene("mic_3_ext", "Trevor stops Michael from smoking (The Wrap Up)"),
		new Cutscene("mic_3_int", "Steve Haines kills Agent Sanchez (The Wrap Up)"),
		new Cutscene("mic_3_int_p1_alt", "Michael meets with Dave Norton, cut cutscene ?? (The Wrap Up)"),
		new Cutscene("mic_3_mcs_1_p1_a2", "Buzzard Shoots at Michael (The Wrap Up)"),
		new Cutscene("mic_4_int", "Michael and Jimmy Meets with Solomon at Meldown premiere (Meltdown)"),
		new Cutscene("paper_1_rcm_alt1", "Franklin Interogates Drayfuss"),
		new Cutscene("pap_1_mcs_1", "papparazi mission Cutscene"),
		new Cutscene("pap_1_rcm", "papparazi mission Cutscene"),
		new Cutscene("pap_2_mcs_1", "papparazi mission Cutscene"),
		new Cutscene("pap_2_rcm_p2", "papparazi mission Cutscene"),
		new Cutscene("pap_3_rcm", "papparazi mission Cutscene"),
		new Cutscene("pap_4_rcm", "papparazi mission Cutscene"),
		new Cutscene("pro_mcs_1", " Prologue Michael Kicks Guard (Prologue)"),
		new Cutscene("pro_mcs_2", "Michael grabs Money (Prologue)"),
		new Cutscene("pro_mcs_3_pt1", "Guard grabs Michael (Prologue)"),
		new Cutscene("pro_mcs_5", "Crew Escapes (Prologue)"),
		new Cutscene("pro_mcs_6", "Trevor Escapes (Prologue) "),
		new Cutscene("pro_mcs_7_concat", "Trevor Escapes (Prologue)"),
		new Cutscene("rbhs_int", "Trevor and Michael Argue at Chefs Meth lab (Paleto Score Setup)"),
		new Cutscene("rbhs_mcs_1", "Michael And Trevor Wait for Lester in Paleto Bay (Paleto Score Setup)"),
		new Cutscene("rbhs_mcs_3", "Michael, Trevor and Lester arive at Chef's methlab (Paleto Score Setup)"),
		new Cutscene("rbhs_msc_3_p3", "Choosing Crew for Paleto Heist (Paleto Score Setup)"),
		new Cutscene("rbh_2ab_mcs_6", "Shootout during Paleto Heist (The Paleto Heist)"),
		new Cutscene("rbh_2a_ext_1", "Agent Sanchez meets with Michael Trevor and Franklin (The Paleto Heist)"),
		new Cutscene("rbh_2a_int", "??"),
		new Cutscene("rbh_2a_mcs_2_p3", "??"),
		new Cutscene("rbh_2a_mcs_2_p7", "??"),
		new Cutscene("rbh_2a_mcs_4", "??"),
		new Cutscene("rbh_2a_mcs_5", "??"),
		new Cutscene("sol_1_ext", "Michael Returns actors to Solomon Richards office (The Ballard of Rocco)"),
		new Cutscene("sol_1_int_alt", "Michael meets with Solomon Richards (The Ballard of Rocco)"),
		new Cutscene("sol_1_mcs_1_concat", "Rocco talks with actors near heli (The Ballard of Rocco)"),
		new Cutscene("sol_1_mcs_2", "Michael beets up Rocco (The Ballard of Rocco)"),
		new Cutscene("sol_1_mcs_3", "Michael threatens Rocco (The Ballard of Rocco)"),
		new Cutscene("sol_2_ext_concat", "Solomon Richards makes Michael executive producer (The Ballard of Rocco)"),
		new Cutscene("sol_2_int_alt1", "Michael Chases Rocco (The Ballard of Rocco)"),
		new Cutscene("sol_3_int", "Michael Meets with Devin weston at Solomon Richards Office"),
		new Cutscene("sol_5_mcs_1", "Michael saves his family (Meltdown)"),
		new Cutscene("sol_5_mcs_2", "Jimmy teabags Michael (Meltdown)"),
		new Cutscene("sol_5_mcs_2_p5", "Michael Talks with Lester at the Peir (Meltdown)"),
		new Cutscene("trevor_1_int", "??"),
		new Cutscene("trevor_2_int", "Ron warns Trevor about the bikers (Crystal Maze)"),
		new Cutscene("trevor_drive_int", "Trevor punches Wade (Crystal Maze)"),
		new Cutscene("trv2_mcs_8", "Trevor Exits plane (Nervous Ron)"),
		new Cutscene("trv_1_mcs_1_p1", "Trevor harrass  Lost MC bikers (Mr. Philips)"),
		new Cutscene("trv_1_mcs_3_concat", "Trevor push Azteca trailer into river (Mr. Philips)"),
		new Cutscene("trv_1_mcs_4", "Trevor orders Ron to exit his vehicle (Mr. Philips)"),
		new Cutscene("trv_2_mcs_4_concat", "heli crashes??  (Nervous Ron)"),
		new Cutscene("trv_2_mcs_6", "Trevor and Ron Steal planes  (Nervous Ron)"),
		new Cutscene("trv_5_ext", "Trevor drops off Wade to stripclub (Hang Ten)"),
		new Cutscene("trv_5_int", "Debora/Floyd Death (Hang Ten)"),
		new Cutscene("trv_dri_ext", "Trevor kicks open Floyd's apt door (Friends Reunited)"),
		new Cutscene("trv_dri_mcs_concat", "Trevor swares about Michael ontop of hill (Friends Reunited)")
	};

	public List<Cutscene> StrangerAndFreaks_Cutscenes = new List<Cutscene>
	{
		new Cutscene("ass_int_2_alt1", "Hotel Assassination"),
		new Cutscene("ass_mcs_1", "Bus Assasstination pre target kill"),
		new Cutscene("bar_1_rcm_p2_t2", "Michael Grass Roots intro"),
		new Cutscene("bar_2_rcm", "Michael Grass Roots outro"),
		new Cutscene("bar_3_rcm", "Trevor Grass Roots intro"),
		new Cutscene("bar_4_rcm", "Trevor Grass Roots outro"),
		new Cutscene("bar_5_rcm_p2", "Franklin Grass Roots intro"),
		new Cutscene("drf_mic_1_cs_1_seq", "FRIEDLANDER Chaos #1"),
		new Cutscene("drf_mic_1_cs_2_seq", "FRIEDLANDER Chaos #2"),
		new Cutscene("drf_mic_2_cs_1_seq", "FRIEDLANDER Evil #1"),
		new Cutscene("drf_mic_2_cs_2_seq", "FRIEDLANDER Evil #2"),
		new Cutscene("drf_mic_3_cs_1_seq", "FRIEDLANDER Abandonment Issues #1"),
		new Cutscene("drf_mic_3_cs_2_seq", " FRIEDLANDER Abandonment Issues #2"),
		new Cutscene("ef_1_rcm", "Exercising Demons Michael"),
		new Cutscene("ef_2_rcm", "Exercising Demons Trevor"),
		new Cutscene("ef_3_rcm", "Exercising Demons Franklin"),
		new Cutscene("ep_1_rcm_seq", "Seeking The Truth"),
		new Cutscene("ep_2_rcm", "Accepting The Truth"),
		new Cutscene("ep_3_rcm_alt1", "Assuming The Truth"),
		new Cutscene("ep_4_rcm", "Chasing The Truth"),
		new Cutscene("eps_4_mcs_1", "Chasing The Truth TV"),
		new Cutscene("eps_4_mcs_2", "Chasing The Truth box"),
		new Cutscene("eps_4_mcs_3", "Chasing The Truth boot"),
		new Cutscene("ep_5_rcm", "Bearing The Truth"),
		new Cutscene("ep_6_rcm", "Delivering The Truth"),
		new Cutscene("ep_7_rcm", "Exercising The Truth"),
		new Cutscene("ep_8_rcm", "Unknowing The Truth"),
		new Cutscene("es_1_rcm_p1", "Risk Assessment intro"),
		new Cutscene("es_1_rcm_concat_seq", "Risk Assessment Dom tree"),
		new Cutscene("es_2_rcm_seq", "Liquidity Risk intro"),
		new Cutscene("es_3_rcm", "Targeted Risk intro"),
		new Cutscene("es_3_mcs_1", "Targeted Risk lose"),
		new Cutscene("es_3_mcs_2", "Targeted Risk win"),
		new Cutscene("es_4_rcm_p1", "Uncalculated Risk intro"),
		new Cutscene("es_4_rcm_p2_seq", "Uncalculated Risk Dom death"),
		new Cutscene("hao_mcs_1", "HAO Shift Work intro"),
		new Cutscene("josh_1_int", "Extra Commission intro"),
		new Cutscene("josh_2_intp1_t4", "Closing The Deal post sex"),
		new Cutscene("josh_3_intp1", "Surreal Estate post sex"),
		new Cutscene("josh_4_int", "Breach of Contract intro"),
		new Cutscene("maude_mcs_1", "Bail Bonds intro"),
		new Cutscene("maude_mcs_2", "Ralph Ostrowski outro"),
		new Cutscene("maude_mcs_3", "Larry Tupper outro"),
		new Cutscene("maude_mcs_4", "Glenn Scoville outro"),
		new Cutscene("maude_mcs_5", "Curtis Weaver outro"),
		new Cutscene("mmb_1_rcm", "The Civil Border Patrol intro"),
		new Cutscene("mmb_2_rcm", "An American Welcome intro"),
		new Cutscene("mmb_3_rcm", "Minute Man Blues intro"),
		new Cutscene("nmt_1_rcm", "Vinewood Souvenirs intro"),
		new Cutscene("nmt_2_rcm_seq", "Vinewood Souvenirs Al Di Napoli intro"),
		new Cutscene("nmt_2_mcs_2", "Vinewood Souvenirs Al Di Napoli kidnap"),
		new Cutscene("nmt_3_rcm", "Vinewood Souvenirs Al Di Napoli pre death"),
		new Cutscene("oscar_mcs_1", "Arms Trafficking Air"),
		new Cutscene("paper_1_rcm_alt1", "A Starlet in Vinewood intro"),
		new Cutscene("pap_1_rcm", "Paparazzo intro"),
		new Cutscene("pap_1_mcs_1", "Paparazzo outro"),
		new Cutscene("pap_2_rcm_p2", "Paparazzo The Sex Tape intro"),
		new Cutscene("pap_2_mcs_1", "Paparazzo The Sex Tape outro"),
		new Cutscene("pap_3_rcm", "Paparazzo The Partnership intro"),
		new Cutscene("pap_4_rcm", " Paparazzo Reality Check"),
		new Cutscene("sas_1_rcm_concat_seq", "The Last One intro"),
		new Cutscene("sas_2_rcm_t7", "The Last One outro"),
		new Cutscene("scrap_1_rcm", "Far Out intro"),
		new Cutscene("scrap_2_rcm_seq", "The Final Frontier intro")
	};

	public NativeMenu MainMenuStuff;

	private ObjectPool modLemonUIObjectPool;

	public ScriptSettings Config;

	private int num = 0;

	public bool RecordCutscene;

	public bool PlayingSPCutscene;

	public static float screenaspectratio;

	public int MenuOpenKey = 178;

	public static int CharacterHash = 2;

	public static int CharacterGender = 1;

	public static bool VehicleWaypointPlaying;

	public Vehicle VehicleA;

	public Vehicle VehicleB;

	private int missionIndex = 1;

	public int TimeOut;

	public static string PlaybackNameA = "mfhlandtitanfinal";

	public static string PlaybackNameB = "mfhlandmogulfinal";

	public Model RequestPed(PedHash Name)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Model result = default(Model);
		((Model)(ref result))._002Ector(Name);
		((Model)(ref result)).Request(10000);
		if (((Model)(ref result)).IsInCdImage && ((Model)(ref result)).IsValid)
		{
			while (!((Model)(ref result)).IsLoaded)
			{
				Script.Wait(50);
			}
			return result;
		}
		((Model)(ref result)).MarkAsNoLongerNeeded();
		return result;
	}

	public void SetPedOutfitCutscene(string MP, Ped NonCutscene)
	{
		if (MP.Equals("MP_1"))
		{
			CutscenePed1Comp[0] = "0_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0));
			CutscenePed1Comp[1] = "1_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1));
			CutscenePed1Comp[2] = "2_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2));
			CutscenePed1Comp[3] = "3_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3));
			CutscenePed1Comp[4] = "4_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4));
			CutscenePed1Comp[5] = "5_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5));
			CutscenePed1Comp[6] = "6_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6));
			CutscenePed1Comp[7] = "7_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7));
			CutscenePed1Comp[8] = "8_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8));
			CutscenePed1Comp[9] = "9_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9));
			CutscenePed1Comp[10] = "10_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10));
			CutscenePed1Comp[11] = "11_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11));
		}
		if (MP.Equals("MP_2"))
		{
			CutscenePed2Comp[0] = "0_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0));
			CutscenePed2Comp[1] = "1_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1));
			CutscenePed2Comp[2] = "2_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2));
			CutscenePed2Comp[3] = "3_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3));
			CutscenePed2Comp[4] = "4_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4));
			CutscenePed2Comp[5] = "5_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5));
			CutscenePed2Comp[6] = "6_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6));
			CutscenePed2Comp[7] = "7_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7));
			CutscenePed2Comp[8] = "8_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8));
			CutscenePed2Comp[9] = "9_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9));
			CutscenePed2Comp[10] = "10_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10));
			CutscenePed2Comp[11] = "11_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11));
		}
		if (MP.Equals("MP_3"))
		{
			CutscenePed3Comp[0] = "0_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0));
			CutscenePed3Comp[1] = "1_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1));
			CutscenePed3Comp[2] = "2_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2));
			CutscenePed3Comp[3] = "3_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3));
			CutscenePed3Comp[4] = "4_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4));
			CutscenePed3Comp[5] = "5_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5));
			CutscenePed3Comp[6] = "6_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6));
			CutscenePed3Comp[7] = "7_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7));
			CutscenePed3Comp[8] = "8_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8));
			CutscenePed3Comp[9] = "9_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9));
			CutscenePed3Comp[10] = "10_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10));
			CutscenePed3Comp[11] = "11_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11));
		}
		if (MP.Equals("MP_4"))
		{
			CutscenePed4Comp[0] = "0_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0));
			CutscenePed4Comp[1] = "1_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1));
			CutscenePed4Comp[2] = "2_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2));
			CutscenePed4Comp[3] = "3_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3));
			CutscenePed4Comp[4] = "4_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4));
			CutscenePed4Comp[5] = "5_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5));
			CutscenePed4Comp[6] = "6_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6));
			CutscenePed4Comp[7] = "7_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7));
			CutscenePed4Comp[8] = "8_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8));
			CutscenePed4Comp[9] = "9_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9));
			CutscenePed4Comp[10] = "10_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10));
			CutscenePed4Comp[11] = "11_" + Function.Call<int>((Hash)7490462606036423932L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11)) + "_" + Function.Call<int>((Hash)334205219021784294L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11));
		}
	}

	public void GetPedOutfitCutscene(string MP, Ped NonCutscene)
	{
		if (MP.Equals("MP_1"))
		{
			string[] array = CutscenePed1Comp[0].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[1].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[2].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[3].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[4].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[5].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[6].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[7].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[8].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[9].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[10].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
			array = CutscenePed1Comp[11].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11), InputArgument.op_Implicit(int.Parse(array[1])), InputArgument.op_Implicit(int.Parse(array[2])), InputArgument.op_Implicit(1));
		}
		if (MP.Equals("MP_2"))
		{
			string[] array2 = CutscenePed2Comp[0].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[1].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[2].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[3].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[4].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[5].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[6].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[7].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[8].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[9].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[10].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
			array2 = CutscenePed2Comp[11].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11), InputArgument.op_Implicit(int.Parse(array2[1])), InputArgument.op_Implicit(int.Parse(array2[2])), InputArgument.op_Implicit(1));
		}
		if (MP.Equals("MP_3"))
		{
			string[] array3 = CutscenePed2Comp[0].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[1].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[2].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[3].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[4].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[5].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[6].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[7].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[8].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[9].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[10].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
			array3 = CutscenePed3Comp[11].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11), InputArgument.op_Implicit(int.Parse(array3[1])), InputArgument.op_Implicit(int.Parse(array3[2])), InputArgument.op_Implicit(1));
		}
		if (MP.Equals("MP_4"))
		{
			string[] array4 = CutscenePed1Comp[0].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(0), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[1].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(1), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[2].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(2), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[3].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(3), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[4].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(4), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[5].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(5), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[6].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(6), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[7].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(7), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[8].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(8), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[9].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(9), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[10].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(10), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
			array4 = CutscenePed4Comp[11].Split('_');
			Function.Call((Hash)2750315038012726912L, Entity.op_Implicit((Entity)(object)NonCutscene), InputArgument.op_Implicit(11), InputArgument.op_Implicit(int.Parse(array4[1])), InputArgument.op_Implicit(int.Parse(array4[2])), InputArgument.op_Implicit(1));
		}
	}

	public static string LoadCutsceneWithFlag(string cutscene, int playbackflag)
	{
		while (!Function.Call<bool>((Hash)(-4206552754506042567L), InputArgument.op_Implicit(cutscene)))
		{
			Function.Call((Hash)(-4450153564967225972L), InputArgument.op_Implicit(cutscene), InputArgument.op_Implicit(playbackflag), InputArgument.op_Implicit(8));
			Script.Yield();
		}
		return cutscene;
	}

	public static string LoadCutscene(string cutscene)
	{
		while (!Function.Call<bool>((Hash)(-4206552754506042567L), InputArgument.op_Implicit(cutscene)))
		{
			Function.Call((Hash)8828871934635515401L, InputArgument.op_Implicit(cutscene));
			Script.Yield();
		}
		return cutscene;
	}

	public void EndCutscene()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		if (RecordCutscene)
		{
			Function.Call((Hash)511811220243728563L);
		}
		PlayerModelSetBack();
		if (PlayerModel == Model.op_Implicit((PedHash)(-1692214353)))
		{
			SetBack(Model.op_Implicit(PlayerModel));
		}
		if (PlayerModel == Model.op_Implicit((PedHash)(-1686040670)))
		{
			SetBack(Model.op_Implicit(PlayerModel));
		}
		if (PlayerModel == Model.op_Implicit((PedHash)225514697))
		{
			SetBack(Model.op_Implicit(PlayerModel));
		}
		if (TeleportBacktoStartPos)
		{
			((Entity)Game.Player.Character).Position = StartPos;
		}
		if ((Entity)(object)CutsenePed2 != (Entity)null && (Entity)(object)NonCutscene2 == (Entity)null)
		{
			((PoolObject)CutsenePed2).Delete();
			CutsenePed2 = null;
		}
		if ((Entity)(object)CutsenePed3 != (Entity)null && (Entity)(object)NonCutscene3 == (Entity)null)
		{
			((PoolObject)CutsenePed3).Delete();
			CutsenePed3 = null;
		}
		if ((Entity)(object)CutsenePed4 != (Entity)null && (Entity)(object)NonCutscene4 == (Entity)null)
		{
			((PoolObject)CutsenePed4).Delete();
			CutsenePed4 = null;
		}
		Script.Wait(1000);
		if ((Entity)(object)NonCutscene2 != (Entity)null)
		{
			((Entity)NonCutscene2).Position = ((Vector3)(ref StartPos)).Around(2f);
		}
		if ((Entity)(object)NonCutscene3 != (Entity)null)
		{
			((Entity)NonCutscene3).Position = ((Vector3)(ref StartPos)).Around(2f);
		}
		if ((Entity)(object)NonCutscene4 != (Entity)null)
		{
			((Entity)NonCutscene4).Position = ((Vector3)(ref StartPos)).Around(2f);
		}
		if ((Entity)(object)NonCutscene2 != (Entity)null)
		{
			Ped nonCutscene = NonCutscene2;
			PedGroup pedGroup = Game.Player.Character.PedGroup;
			Function.Call((Hash)(-6974808092771143243L), Entity.op_Implicit((Entity)(object)nonCutscene), PedGroup.op_Implicit(pedGroup));
			Function.Call((Hash)(-4079649877180351064L), Entity.op_Implicit((Entity)(object)nonCutscene), InputArgument.op_Implicit(100));
		}
		if ((Entity)(object)NonCutscene3 != (Entity)null)
		{
			Ped nonCutscene2 = NonCutscene3;
			PedGroup pedGroup2 = Game.Player.Character.PedGroup;
			Function.Call((Hash)(-6974808092771143243L), Entity.op_Implicit((Entity)(object)nonCutscene2), PedGroup.op_Implicit(pedGroup2));
			Function.Call((Hash)(-4079649877180351064L), Entity.op_Implicit((Entity)(object)nonCutscene2), InputArgument.op_Implicit(100));
		}
		if ((Entity)(object)NonCutscene4 != (Entity)null)
		{
			Ped nonCutscene3 = NonCutscene4;
			PedGroup pedGroup3 = Game.Player.Character.PedGroup;
			Function.Call((Hash)(-6974808092771143243L), Entity.op_Implicit((Entity)(object)nonCutscene3), PedGroup.op_Implicit(pedGroup3));
			Function.Call((Hash)(-4079649877180351064L), Entity.op_Implicit((Entity)(object)nonCutscene3), InputArgument.op_Implicit(100));
		}
		Function.Call((Hash)(-3305433416267445730L));
		PlayingCutscene = false;
		foreach (Ped nonMainChar in NonMainChars)
		{
			if ((Entity)(object)nonMainChar != (Entity)null)
			{
				((PoolObject)nonMainChar).Delete();
			}
		}
	}

	public Model RequestModel(string Name)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Model result = default(Model);
		((Model)(ref result))._002Ector(Name);
		((Model)(ref result)).Request(250);
		if (((Model)(ref result)).IsInCdImage && ((Model)(ref result)).IsValid)
		{
			while (!((Model)(ref result)).IsLoaded)
			{
				Script.Wait(50);
			}
			return result;
		}
		((Model)(ref result)).MarkAsNoLongerNeeded();
		return result;
	}

	public Model RequestModel(PedHash Name)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Model result = default(Model);
		((Model)(ref result))._002Ector(Name);
		((Model)(ref result)).Request(250);
		if (((Model)(ref result)).IsInCdImage && ((Model)(ref result)).IsValid)
		{
			while (!((Model)(ref result)).IsLoaded)
			{
				Script.Wait(50);
			}
			return result;
		}
		((Model)(ref result)).MarkAsNoLongerNeeded();
		return result;
	}

	public void StartCutscene()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Invalid comparison between Unknown and I4
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		Function.Call<int>((Hash)8828871934635515401L, InputArgument.op_Implicit(CurrentCutsene));
		if (!Function.Call<bool>((Hash)2489713879041514556L, InputArgument.op_Implicit(CurrentCutsene)) || Function.Call<bool>((Hash)(-3187737643600379106L)))
		{
			return;
		}
		StartPos = ((Entity)Game.Player.Character).Position;
		cutscene = Function.Call<int>((Hash)8828871934635515401L, InputArgument.op_Implicit(CurrentCutsene));
		Function.Call<int>((Hash)8828871934635515401L, InputArgument.op_Implicit(CurrentCutsene));
		while (!Function.Call<bool>((Hash)(-4206552754506042567L)))
		{
			Script.Yield();
		}
		if (!Function.Call<bool>((Hash)(-4206552754506042567L)))
		{
			return;
		}
		int num = Function.Call<int>((Hash)(-1053031750656612747L), Entity.op_Implicit((Entity)(object)Game.Player.Character));
		NonCutscene2 = Function.Call<Ped>((Hash)5856179815557885335L, InputArgument.op_Implicit(num), InputArgument.op_Implicit(0));
		NonCutscene3 = Function.Call<Ped>((Hash)5856179815557885335L, InputArgument.op_Implicit(num), InputArgument.op_Implicit(1));
		NonCutscene4 = Function.Call<Ped>((Hash)5856179815557885335L, InputArgument.op_Implicit(num), InputArgument.op_Implicit(2));
		PlayerModel = ((Entity)Game.Player.Character).Model;
		PlayerModelSet();
		if ((Entity)(object)NonCutscene2 != (Entity)null)
		{
			Function.Call((Hash)(-1336442639748928574L), Entity.op_Implicit((Entity)(object)NonCutscene2));
		}
		if ((Entity)(object)NonCutscene3 != (Entity)null)
		{
			Function.Call((Hash)(-1336442639748928574L), Entity.op_Implicit((Entity)(object)NonCutscene3));
		}
		if ((Entity)(object)NonCutscene4 != (Entity)null)
		{
			Function.Call((Hash)(-1336442639748928574L), Entity.op_Implicit((Entity)(object)NonCutscene4));
		}
		if (!PlayingSPCutscene)
		{
			if (RepalceMp1)
			{
				CutsenePed1 = Game.Player.Character;
				SetPedOutfitCutscene("MP_1", Game.Player.Character);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("MP_1"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			if (RepalceMpMFwithPlayer)
			{
				CutsenePed1 = Game.Player.Character;
				if ((int)Game.Player.Character.Gender == 0)
				{
					SetPedOutfitCutscene("MP_Male_Character", Game.Player.Character);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("MP_Male_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					Ped val = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(0f, 0f, 0f), 0f);
					((Entity)val).IsVisible = false;
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val), InputArgument.op_Implicit("MP_Female_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					NonMainChars.Add(val);
				}
				else
				{
					SetPedOutfitCutscene("MP_Female_Character", Game.Player.Character);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("MP_Female_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					Ped val2 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(0f, 0f, 0f), 0f);
					((Entity)val2).IsVisible = false;
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val2), InputArgument.op_Implicit("MP_Male_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					NonMainChars.Add(val2);
				}
			}
			if (HideNonMainChars)
			{
				Ped val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_1"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_5"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_6"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_7"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
				((Entity)val3).IsVisible = false;
				NonMainChars.Add(val3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_8"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			if ((Entity)(object)Game.Player.Character.LastVehicle != (Entity)null)
			{
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)Game.Player.Character.LastVehicle), InputArgument.op_Implicit("MPH_Car"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)Game.Player.Character.LastVehicle).IsVisible = false;
			}
			if (RepalceMp2)
			{
				if ((Entity)(object)NonCutscene2 != (Entity)null && !NonCutscene2.Exists())
				{
					NonCutscene2 = null;
				}
				if ((Entity)(object)NonCutscene2 != (Entity)null)
				{
					SetPedOutfitCutscene("MP_2", NonCutscene2);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene2), InputArgument.op_Implicit("MP_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				}
				else
				{
					CutsenePed2 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed2), InputArgument.op_Implicit("MP_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					((Entity)CutsenePed2).IsVisible = false;
					Notification.Show("Creating Cutscene Ped 2", false);
				}
			}
			else
			{
				CutsenePed2 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed2), InputArgument.op_Implicit("MP_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)CutsenePed2).IsVisible = false;
				Notification.Show("Creating Cutscene Ped 2", false);
			}
			if (RepalceMp3)
			{
				if ((Entity)(object)NonCutscene3 != (Entity)null && !NonCutscene3.Exists())
				{
					NonCutscene3 = null;
				}
				if ((Entity)(object)NonCutscene3 != (Entity)null)
				{
					SetPedOutfitCutscene("MP_3", NonCutscene3);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene3), InputArgument.op_Implicit("MP_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				}
				else
				{
					CutsenePed3 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed3), InputArgument.op_Implicit("MP_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					((Entity)CutsenePed3).IsVisible = false;
				}
			}
			else
			{
				CutsenePed3 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed3), InputArgument.op_Implicit("MP_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)CutsenePed3).IsVisible = false;
			}
			if (RepalceMp4)
			{
				if ((Entity)(object)NonCutscene4 != (Entity)null && !NonCutscene4.Exists())
				{
					NonCutscene4 = null;
				}
				if ((Entity)(object)NonCutscene4 != (Entity)null)
				{
					SetPedOutfitCutscene("MP_4", NonCutscene4);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene4), InputArgument.op_Implicit("MP_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				}
				else
				{
					CutsenePed4 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
					Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed4), InputArgument.op_Implicit("MP_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
					((Entity)CutsenePed4).IsVisible = false;
				}
			}
			else
			{
				CutsenePed4 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed4), InputArgument.op_Implicit("MP_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)CutsenePed4).IsVisible = false;
			}
		}
		else
		{
			if (RepalceMp1)
			{
				CutsenePed1 = Game.Player.Character;
				SetPedOutfitCutscene("Franklin", Game.Player.Character);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("Franklin"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			if (RepalceMp2)
			{
				CutsenePed1 = Game.Player.Character;
				SetPedOutfitCutscene("Michael", Game.Player.Character);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("Michael"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			if (RepalceMp3)
			{
				CutsenePed1 = Game.Player.Character;
				SetPedOutfitCutscene("Trevor", Game.Player.Character);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("Franklin"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
		}
		if ((Entity)(object)NonCutscene2 != (Entity)null && NonCutscene2.Exists())
		{
			((Entity)NonCutscene2).Position = ((Entity)Game.Player.Character).Position;
			Notification.Show("BodyguardA is Active", false);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene2), InputArgument.op_Implicit("MP_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
		}
		if ((Entity)(object)NonCutscene3 != (Entity)null && NonCutscene3.Exists())
		{
			((Entity)NonCutscene3).Position = ((Entity)Game.Player.Character).Position;
			Notification.Show("BodyguardB is Active", false);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene3), InputArgument.op_Implicit("MP_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
		}
		if ((Entity)(object)NonCutscene4 != (Entity)null && NonCutscene4.Exists())
		{
			((Entity)NonCutscene4).Position = ((Entity)Game.Player.Character).Position;
			Notification.Show("BodyguardC is Active", false);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene4), InputArgument.op_Implicit("MP_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
		}
		Function.Call((Hash)1760164965717704571L, InputArgument.op_Implicit(cutscene));
		if (RecordCutscene)
		{
			Function.Call((Hash)(-4347046765538399103L), InputArgument.op_Implicit(1));
		}
		if (!SetCutsceneFadeout)
		{
			Function.Call((Hash)(-9181728881222301740L), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false));
		}
		if (SetCutsceneFadeout)
		{
			Function.Call((Hash)(-9181728881222301740L), InputArgument.op_Implicit(true), InputArgument.op_Implicit(true), InputArgument.op_Implicit(true), InputArgument.op_Implicit(true));
		}
		Script.Wait(100);
		PlayingCutscene = true;
		if (!PlayingSPCutscene)
		{
			GetPedOutfitCutscene("MP_1", CutsenePed1);
			if (RepalceMp1 && (Entity)(object)NonCutscene2 != (Entity)null)
			{
				GetPedOutfitCutscene("MP_2", NonCutscene2);
			}
			if (RepalceMp3 && (Entity)(object)NonCutscene3 != (Entity)null)
			{
				GetPedOutfitCutscene("MP_3", NonCutscene3);
			}
			if (RepalceMp4 && (Entity)(object)NonCutscene4 != (Entity)null)
			{
				GetPedOutfitCutscene("MP_4", NonCutscene4);
			}
		}
		else
		{
			if (RepalceMp1)
			{
				GetPedOutfitCutscene("Franklin", Game.Player.Character);
			}
			if (RepalceMp2)
			{
				GetPedOutfitCutscene("Michael", Game.Player.Character);
			}
			if (RepalceMp3)
			{
				GetPedOutfitCutscene("Trevor", Game.Player.Character);
			}
		}
	}

	public void StartCutscene(Vector3 X)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Invalid comparison between Unknown and I4
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		Function.Call<int>((Hash)8828871934635515401L, InputArgument.op_Implicit(CurrentCutsene));
		if (!Function.Call<bool>((Hash)2489713879041514556L, InputArgument.op_Implicit(CurrentCutsene)) || Function.Call<bool>((Hash)(-3187737643600379106L)))
		{
			return;
		}
		CurrentPlayingCutscene = CurrentCutsene;
		StartPos = ((Entity)Game.Player.Character).Position;
		cutscene = Function.Call<int>((Hash)8828871934635515401L, InputArgument.op_Implicit(CurrentCutsene));
		Function.Call<int>((Hash)8828871934635515401L, InputArgument.op_Implicit(CurrentCutsene));
		while (!Function.Call<bool>((Hash)(-4206552754506042567L)))
		{
			Script.Yield();
		}
		if (!Function.Call<bool>((Hash)(-4206552754506042567L)))
		{
			return;
		}
		int num = Function.Call<int>((Hash)(-1053031750656612747L), Entity.op_Implicit((Entity)(object)Game.Player.Character));
		NonCutscene2 = Function.Call<Ped>((Hash)5856179815557885335L, InputArgument.op_Implicit(num), InputArgument.op_Implicit(0));
		NonCutscene3 = Function.Call<Ped>((Hash)5856179815557885335L, InputArgument.op_Implicit(num), InputArgument.op_Implicit(1));
		NonCutscene4 = Function.Call<Ped>((Hash)5856179815557885335L, InputArgument.op_Implicit(num), InputArgument.op_Implicit(2));
		PlayerModel = ((Entity)Game.Player.Character).Model;
		PlayerModelSet();
		if (RepalceMp1)
		{
			CutsenePed1 = Game.Player.Character;
			SetPedOutfitCutscene("MP_1", Game.Player.Character);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("MP_1"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
		}
		if (RepalceMpMFwithPlayer)
		{
			CutsenePed1 = Game.Player.Character;
			if ((int)Game.Player.Character.Gender == 0)
			{
				SetPedOutfitCutscene("MP_Male_Character", Game.Player.Character);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("MP_Male_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				Ped val = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(0f, 0f, 0f), 0f);
				((Entity)val).IsVisible = false;
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val), InputArgument.op_Implicit("MP_Female_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				NonMainChars.Add(val);
			}
			else
			{
				SetPedOutfitCutscene("MP_Female_Character", Game.Player.Character);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed1), InputArgument.op_Implicit("MP_Female_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				Ped val2 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(0f, 0f, 0f), 0f);
				((Entity)val2).IsVisible = false;
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val2), InputArgument.op_Implicit("MP_Male_Character"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				NonMainChars.Add(val2);
			}
		}
		if (HideNonMainChars)
		{
			Ped val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_1"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_5"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_6"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_7"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			val3 = World.CreateRandomPed(new Vector3(-1188.608f, -1644.653f, 4.373932f));
			((Entity)val3).IsVisible = false;
			NonMainChars.Add(val3);
			Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)val3), InputArgument.op_Implicit("MP_Plane_Passenger_8"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
		}
		if (RepalceMp2)
		{
			if ((Entity)(object)NonCutscene2 != (Entity)null && !NonCutscene2.Exists())
			{
				Notification.Show("Cutscene Ped 2 Does not Exist", false);
				NonCutscene2 = null;
			}
			if ((Entity)(object)NonCutscene2 != (Entity)null)
			{
				SetPedOutfitCutscene("MP_2", NonCutscene2);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene2), InputArgument.op_Implicit("MP_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			else
			{
				CutsenePed2 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed2), InputArgument.op_Implicit("MP_2"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)CutsenePed2).IsVisible = false;
			}
		}
		if (RepalceMp3)
		{
			if ((Entity)(object)NonCutscene3 != (Entity)null && !NonCutscene3.Exists())
			{
				Notification.Show("Cutscene Ped 3 Does not Exist", false);
				NonCutscene3 = null;
			}
			if ((Entity)(object)NonCutscene3 != (Entity)null)
			{
				SetPedOutfitCutscene("MP_3", NonCutscene3);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene3), InputArgument.op_Implicit("MP_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			else
			{
				CutsenePed3 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed3), InputArgument.op_Implicit("MP_3"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)CutsenePed3).IsVisible = false;
			}
		}
		if (RepalceMp4)
		{
			if ((Entity)(object)NonCutscene4 != (Entity)null && !NonCutscene4.Exists())
			{
				Notification.Show("Cutscene Ped 4 Does not Exist", false);
				NonCutscene4 = null;
			}
			if ((Entity)(object)NonCutscene4 != (Entity)null)
			{
				SetPedOutfitCutscene("MP_4", NonCutscene4);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)NonCutscene4), InputArgument.op_Implicit("MP_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
			}
			else
			{
				CutsenePed4 = World.CreatePed(RequestModel((PedHash)1885233650), new Vector3(2488.348f, -267.3637f, -71.64563f), 0f);
				Function.Call((Hash)(-2014203773897555224L), Entity.op_Implicit((Entity)(object)CutsenePed4), InputArgument.op_Implicit("MP_4"), InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(64));
				((Entity)CutsenePed4).IsVisible = false;
			}
		}
		if ((Entity)(object)NonCutscene2 != (Entity)null)
		{
			Function.Call((Hash)(-1336442639748928574L), Entity.op_Implicit((Entity)(object)NonCutscene2));
		}
		if ((Entity)(object)NonCutscene3 != (Entity)null)
		{
			Function.Call((Hash)(-1336442639748928574L), Entity.op_Implicit((Entity)(object)NonCutscene3));
		}
		if ((Entity)(object)NonCutscene4 != (Entity)null)
		{
			Function.Call((Hash)(-1336442639748928574L), Entity.op_Implicit((Entity)(object)NonCutscene4));
		}
		Function.Call((Hash)2061203472233668543L, InputArgument.op_Implicit(X.X), InputArgument.op_Implicit(X.Y), InputArgument.op_Implicit(X.Z - 1f), InputArgument.op_Implicit(0));
		if (RecordCutscene)
		{
			Function.Call((Hash)(-4347046765538399103L), InputArgument.op_Implicit(1));
		}
		if (!SetCutsceneFadeout)
		{
			Function.Call((Hash)(-9181728881222301740L), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false));
		}
		if (SetCutsceneFadeout)
		{
			Function.Call((Hash)(-9181728881222301740L), InputArgument.op_Implicit(true), InputArgument.op_Implicit(true), InputArgument.op_Implicit(true), InputArgument.op_Implicit(true));
		}
		Script.Wait(100);
		PlayingCutscene = true;
		GetPedOutfitCutscene("MP_1", CutsenePed1);
		if (RepalceMp1 && (Entity)(object)NonCutscene2 != (Entity)null)
		{
			GetPedOutfitCutscene("MP_2", NonCutscene2);
		}
		if (RepalceMp3 && (Entity)(object)NonCutscene3 != (Entity)null)
		{
			GetPedOutfitCutscene("MP_3", NonCutscene3);
		}
		if (RepalceMp4 && (Entity)(object)NonCutscene4 != (Entity)null)
		{
			GetPedOutfitCutscene("MP_4", NonCutscene4);
		}
	}

	public void StopCutscene()
	{
		Function.Call((Hash)(-3305433416267445730L));
		PlayingCutscene = false;
		Function.Call((Hash)(-3454974875737139118L), InputArgument.op_Implicit(CurrentPlayingCutscene));
	}

	public void LoadIniFile(string iniName)
	{
		try
		{
			Config = ScriptSettings.Load(iniName);
			MenuOpenKey = Config.GetValue<int>("Setup", "MenuOpenKey", MenuOpenKey);
		}
		catch (Exception)
		{
			Notification.Show("~r~Error~w~: " + iniName + " Failed To Load.", false);
		}
	}

	public Class1()
	{
		//IL_3eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ee4: Expected O, but got Unknown
		//IL_3eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef9: Expected O, but got Unknown
		//IL_3f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f20: Expected O, but got Unknown
		//IL_3f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f59: Expected O, but got Unknown
		//IL_3f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa3: Expected O, but got Unknown
		//IL_3feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff5: Expected O, but got Unknown
		//IL_4003: Unknown result type (might be due to invalid IL or missing references)
		//IL_400d: Expected O, but got Unknown
		//IL_401b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4025: Expected O, but got Unknown
		//IL_4064: Unknown result type (might be due to invalid IL or missing references)
		//IL_406b: Expected O, but got Unknown
		//IL_40a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_40aa: Expected O, but got Unknown
		//IL_40d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e0: Expected O, but got Unknown
		//IL_411d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4127: Expected O, but got Unknown
		//IL_4164: Unknown result type (might be due to invalid IL or missing references)
		//IL_416e: Expected O, but got Unknown
		//IL_419a: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a4: Expected O, but got Unknown
		//IL_41d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41da: Expected O, but got Unknown
		//IL_4206: Unknown result type (might be due to invalid IL or missing references)
		//IL_4210: Expected O, but got Unknown
		//IL_423b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4242: Expected O, but got Unknown
		//IL_4265: Unknown result type (might be due to invalid IL or missing references)
		//IL_426c: Expected O, but got Unknown
		//IL_428f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4296: Expected O, but got Unknown
		//IL_42ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c4: Expected O, but got Unknown
		//IL_42f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_42fa: Expected O, but got Unknown
		//IL_4382: Unknown result type (might be due to invalid IL or missing references)
		//IL_4389: Expected O, but got Unknown
		//IL_43c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_43cb: Expected O, but got Unknown
		//IL_43f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4400: Expected O, but got Unknown
		//IL_4423: Unknown result type (might be due to invalid IL or missing references)
		//IL_442a: Expected O, but got Unknown
		//IL_444d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4454: Expected O, but got Unknown
		//IL_4479: Unknown result type (might be due to invalid IL or missing references)
		//IL_4483: Expected O, but got Unknown
		//IL_44b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_44bd: Expected O, but got Unknown
		//IL_4548: Unknown result type (might be due to invalid IL or missing references)
		//IL_454f: Expected O, but got Unknown
		//IL_4587: Unknown result type (might be due to invalid IL or missing references)
		//IL_4591: Expected O, but got Unknown
		//IL_45bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_45c6: Expected O, but got Unknown
		//IL_45e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f0: Expected O, but got Unknown
		//IL_4613: Unknown result type (might be due to invalid IL or missing references)
		//IL_461a: Expected O, but got Unknown
		//IL_463f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4649: Expected O, but got Unknown
		//IL_4679: Unknown result type (might be due to invalid IL or missing references)
		//IL_4683: Expected O, but got Unknown
		Notification.Show("~g~CutscenePlayer\n~w~ Loaded, Requested by ~y~Chadoy", false);
		LoadIniFile("scripts\\CutscenePlayer.ini");
		modLemonUIObjectPool = new ObjectPool();
		MainMenuStuff = new NativeMenu("Cutscene Menu", "Select an Option");
		modLemonUIObjectPool.Add((IProcessable)(object)MainMenuStuff);
		NativeMenu Story = new NativeMenu("Cutscene Menu", "Story Cutscenes");
		modLemonUIObjectPool.Add((IProcessable)(object)Story);
		MainMenuStuff.AddSubMenu(Story);
		NativeMenu StFrea = new NativeMenu("Cutscene Menu", "Stanger & Freaks Cutscenes");
		modLemonUIObjectPool.Add((IProcessable)(object)StFrea);
		MainMenuStuff.AddSubMenu(StFrea);
		StFrea.Description = "";
		NativeMenu Online = new NativeMenu("Cutscene Menu", "Online Cutscenes");
		modLemonUIObjectPool.Add((IProcessable)(object)Online);
		MainMenuStuff.AddSubMenu(Online);
		CurrentCutsene = OnlineCutscenes[0].Name;
		Online.SelectedIndexChanged += (SelectedEventHandler)delegate
		{
			CurrentCutsene = OnlineCutscenes[Online.SelectedIndex].Name;
		};
		Story.SelectedIndexChanged += (SelectedEventHandler)delegate
		{
			CurrentCutsene = StoryCutscenes[Story.SelectedIndex].Name;
		};
		StFrea.SelectedIndexChanged += (SelectedEventHandler)delegate
		{
			CurrentCutsene = StrangerAndFreaks_Cutscenes[StFrea.SelectedIndex].Name;
		};
		foreach (Cutscene onlineCutscene in OnlineCutscenes)
		{
			NativeMenu val = new NativeMenu(onlineCutscene.Name.ToUpper(), onlineCutscene.Name.ToUpper());
			Online.AddSubMenu(val);
			val.Description = onlineCutscene.Description;
			modLemonUIObjectPool.Add((IProcessable)(object)val);
			NativeCheckboxItem Mp1 = new NativeCheckboxItem("Replace MP1 With Player");
			val.Add((NativeItem)(object)Mp1);
			Mp1.CheckboxChanged += delegate
			{
				RepalceMp1 = Mp1.Checked;
				Notification.Show("Replace MP1 Ped in Cutscene with Player set to " + RepalceMp1, false);
			};
			NativeCheckboxItem MpIntro1 = new NativeCheckboxItem("Replace MP Male/Female With Player");
			val.Add((NativeItem)(object)MpIntro1);
			((NativeItem)MpIntro1).Description = "Only Works in MP_INTRO_CONCAT (GTAO Intro Cutscene)";
			MpIntro1.CheckboxChanged += delegate
			{
				RepalceMpMFwithPlayer = MpIntro1.Checked;
				Notification.Show("Replace MP Male/Female Ped in Cutscene with Player set to " + RepalceMpMFwithPlayer, false);
			};
			NativeCheckboxItem HidenonMain = new NativeCheckboxItem("Hide Non Main Peds in Cutscene");
			val.Add((NativeItem)(object)HidenonMain);
			((NativeItem)MpIntro1).Description = "Only Works in MP_INTRO_CONCAT (GTAO Intro Cutscene)";
			HidenonMain.CheckboxChanged += delegate
			{
				HideNonMainChars = HidenonMain.Checked;
				Notification.Show("Hide Non Main Peds in Cutscene with Player set to " + HideNonMainChars, false);
			};
			NativeCheckboxItem Mp2 = new NativeCheckboxItem("Replace MP2 With 1st Bodyguard");
			val.Add((NativeItem)(object)Mp2);
			((NativeItem)Mp2).Activated += delegate
			{
				RepalceMp2 = Mp2.Checked;
				Notification.Show("Replace MP1 Ped in Cutscene with 1st Bodyguard/Companion set to " + RepalceMp2, false);
			};
			NativeCheckboxItem Mp3 = new NativeCheckboxItem("Replace MP3 With 2nd Bodyguard");
			val.Add((NativeItem)(object)Mp3);
			((NativeItem)Mp3).Activated += delegate
			{
				RepalceMp3 = Mp3.Checked;
				Notification.Show("Replace MP1 Ped in Cutscene with 2nd Bodyguard set to " + RepalceMp3, false);
			};
			NativeCheckboxItem Mp4 = new NativeCheckboxItem("Replace MP4 With 3rd Bodyguard");
			val.Add((NativeItem)(object)Mp4);
			((NativeItem)Mp4).Activated += delegate
			{
				RepalceMp4 = Mp4.Checked;
				Notification.Show("Replace MP1 Ped in Cutscene with 3rd Bodyguard set to " + RepalceMp4, false);
			};
			NativeCheckboxItem Record3 = new NativeCheckboxItem("Record Cutscene");
			val.Add((NativeItem)(object)Record3);
			Record3.CheckboxChanged += delegate
			{
				RecordCutscene = Record3.Checked;
				Notification.Show("Record Cutscene set to " + RecordCutscene, false);
			};
			NativeItem val2 = new NativeItem("Play Cutscene");
			val.Add(val2);
			val2.Activated += delegate
			{
				PlayingSPCutscene = false;
				StartCutscene();
			};
			NativeItem val3 = new NativeItem("Play Cutscene At Player Coords");
			val.Add(val3);
			val3.Activated += delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				PlayingSPCutscene = false;
				Vector3 position3 = ((Entity)Game.Player.Character).Position;
				StartCutscene(position3);
			};
			NativeItem val4 = new NativeItem("Stop Cutscene");
			val.Add(val4);
			val4.Activated += delegate
			{
				EndCutscene();
			};
			NativeCheckboxItem TpStart3 = new NativeCheckboxItem("Teleport back to Start Position");
			val.Add((NativeItem)(object)TpStart3);
			((NativeItem)TpStart3).Activated += delegate
			{
				TeleportBacktoStartPos = TpStart3.Checked;
				Notification.Show("Teleport back to players Position before hitting Start Cutscene set to " + TeleportBacktoStartPos, false);
			};
			NativeCheckboxItem SetCutsceneNoFadeout3 = new NativeCheckboxItem("Set Cutscene Fadeout");
			val.Add((NativeItem)(object)SetCutsceneNoFadeout3);
			((NativeItem)SetCutsceneNoFadeout3).Activated += delegate
			{
				SetCutsceneFadeout = SetCutsceneNoFadeout3.Checked;
				Notification.Show("Set Cutscene Fadeout set to " + SetCutsceneFadeout, false);
			};
		}
		foreach (Cutscene storyCutscene in StoryCutscenes)
		{
			NativeMenu val5 = new NativeMenu(storyCutscene.Name.ToUpper(), storyCutscene.Name.ToUpper());
			Story.AddSubMenu(val5);
			val5.Description = storyCutscene.Description;
			modLemonUIObjectPool.Add((IProcessable)(object)val5);
			NativeCheckboxItem Record2 = new NativeCheckboxItem("Record Cutscene");
			val5.Add((NativeItem)(object)Record2);
			Record2.CheckboxChanged += delegate
			{
				RecordCutscene = Record2.Checked;
				Notification.Show("Record Cutscene set to " + RecordCutscene, false);
			};
			NativeItem val6 = new NativeItem("Play Cutscene");
			val5.Add(val6);
			val6.Activated += delegate
			{
				PlayingSPCutscene = true;
				StartCutscene();
			};
			NativeItem val7 = new NativeItem("Play Cutscene At Player Coords");
			val5.Add(val7);
			val7.Activated += delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				PlayingSPCutscene = true;
				Vector3 position2 = ((Entity)Game.Player.Character).Position;
				StartCutscene(position2);
			};
			NativeItem val8 = new NativeItem("Stop Cutscene");
			val5.Add(val8);
			val8.Activated += delegate
			{
				EndCutscene();
			};
			NativeCheckboxItem TpStart2 = new NativeCheckboxItem("Teleport back to Start Position");
			val5.Add((NativeItem)(object)TpStart2);
			((NativeItem)TpStart2).Activated += delegate
			{
				TeleportBacktoStartPos = TpStart2.Checked;
				Notification.Show("Teleport back to players Position before hitting Start Cutscene set to " + TeleportBacktoStartPos, false);
			};
			NativeCheckboxItem SetCutsceneNoFadeout2 = new NativeCheckboxItem("Set Cutscene Fadeout");
			val5.Add((NativeItem)(object)SetCutsceneNoFadeout2);
			((NativeItem)SetCutsceneNoFadeout2).Activated += delegate
			{
				SetCutsceneFadeout = SetCutsceneNoFadeout2.Checked;
				Notification.Show("Set Cutscene Fadeout set to " + SetCutsceneFadeout, false);
			};
		}
		foreach (Cutscene strangerAndFreaks_Cutscene in StrangerAndFreaks_Cutscenes)
		{
			NativeMenu val9 = new NativeMenu(strangerAndFreaks_Cutscene.Name.ToUpper(), strangerAndFreaks_Cutscene.Name.ToUpper());
			StFrea.AddSubMenu(val9);
			val9.Description = strangerAndFreaks_Cutscene.Description;
			modLemonUIObjectPool.Add((IProcessable)(object)val9);
			NativeCheckboxItem Record = new NativeCheckboxItem("Record Cutscene");
			val9.Add((NativeItem)(object)Record);
			Record.CheckboxChanged += delegate
			{
				RecordCutscene = Record.Checked;
				Notification.Show("Record Cutscene set to " + RecordCutscene, false);
			};
			NativeItem val10 = new NativeItem("Play Cutscene");
			val9.Add(val10);
			val10.Activated += delegate
			{
				PlayingSPCutscene = true;
				StartCutscene();
			};
			NativeItem val11 = new NativeItem("Play Cutscene At Player Coords");
			val9.Add(val11);
			val11.Activated += delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				PlayingSPCutscene = true;
				Vector3 position = ((Entity)Game.Player.Character).Position;
				StartCutscene(position);
			};
			NativeItem val12 = new NativeItem("Stop Cutscene");
			val9.Add(val12);
			val12.Activated += delegate
			{
				EndCutscene();
			};
			NativeCheckboxItem TpStart = new NativeCheckboxItem("Teleport back to Start Position");
			val9.Add((NativeItem)(object)TpStart);
			((NativeItem)TpStart).Activated += delegate
			{
				TeleportBacktoStartPos = TpStart.Checked;
				Notification.Show("Teleport back to players Position before hitting Start Cutscene set to " + TeleportBacktoStartPos, false);
			};
			NativeCheckboxItem SetCutsceneNoFadeout = new NativeCheckboxItem("Set Cutscene Fadeout");
			val9.Add((NativeItem)(object)SetCutsceneNoFadeout);
			((NativeItem)SetCutsceneNoFadeout).Activated += delegate
			{
				SetCutsceneFadeout = SetCutsceneNoFadeout.Checked;
				Notification.Show("Set Cutscene Fadeout set to " + SetCutsceneFadeout, false);
			};
		}
		((Script)this).Aborted += OnShutdown;
		((Script)this).Tick += OnTick;
	}

	private Vector2 World3DToScreen2d(Vector3 pos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		OutputArgument val = new OutputArgument();
		OutputArgument val2 = new OutputArgument();
		Function.Call<bool>((Hash)3812348786834502901L, InputArgument.op_Implicit(pos.X), InputArgument.op_Implicit(pos.Y), InputArgument.op_Implicit(pos.Z), InputArgument.op_Implicit(val), InputArgument.op_Implicit(val2));
		return new Vector2(val.GetResult<float>(), val2.GetResult<float>());
	}

	public static float GetRatio(float num)
	{
		return 0.5f - (0.5f - num) / screenaspectratio;
	}

	public static void drawText(string text, float x, float y, float scale, int r, int g, int b, int a, int Font)
	{
		if (a < 0)
		{
			a = 0;
		}
		if (a > 255)
		{
			a = 255;
		}
		screenaspectratio = Function.Call<float>((Hash)(-1067213516044759673L), InputArgument.op_Implicit(true));
		Function.Call((Hash)2736978246810207435L, InputArgument.op_Implicit("STRING"));
		Function.Call((Hash)7789129354908300458L, InputArgument.op_Implicit(text));
		Function.Call((Hash)(-4725643803099155390L), InputArgument.op_Implicit(r), InputArgument.op_Implicit(g), InputArgument.op_Implicit(b), InputArgument.op_Implicit(a));
		Function.Call((Hash)560759698880214217L, InputArgument.op_Implicit(0f), InputArgument.op_Implicit(scale));
		Function.Call((Hash)7412968334783068634L, InputArgument.op_Implicit(Font));
		Function.Call((Hash)(-3674552073055540649L), InputArgument.op_Implicit(GetRatio(x)), InputArgument.op_Implicit(y), InputArgument.op_Implicit(0.1));
	}

	private unsafe void PlayerModelSet()
	{
		if (CharacterHash == 1)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 225514697L;
		}
		else if (CharacterHash == 2)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 2602752943L;
		}
		else if (CharacterHash == 3)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 2608926626L;
		}
	}

	private unsafe void PlayerModelSetBack()
	{
		if (CharacterGender == 1)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 1885233650L;
		}
		else if (CharacterGender == 2)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 2627665880L;
		}
	}

	private unsafe void SetBack(PedHash Model)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		if ((int)Model == 225514697)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 225514697L;
		}
		if ((int)Model == -1692214353)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 2602752943L;
		}
		if ((int)Model == -1686040670 && CharacterHash == 3)
		{
			*(long*)(*(long*)((long)((Entity)Game.Player.Character).MemoryAddress + 32) + 24) = 2608926626L;
		}
	}

	public static void DisplayHelpTextThisFrame(string helpText, int Shape)
	{
		Function.Call((Hash)(-8860350453193909743L), InputArgument.op_Implicit("CELL_EMAIL_BCON"));
		int num = 99;
		for (int i = 0; i < helpText.Length; i += num)
		{
			Function.Call((Hash)7789129354908300458L, InputArgument.op_Implicit(helpText.Substring(i, Math.Min(num, helpText.Length - i))));
		}
		Function.Call((Hash)2562546386151446694L, InputArgument.op_Implicit(0), InputArgument.op_Implicit(0), InputArgument.op_Implicit(1), InputArgument.op_Implicit(Shape));
	}

	public void OnTick(object sender, EventArgs e)
	{
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Invalid comparison between Unknown and I4
		if (Game.IsControlJustPressed((Control)73))
		{
		}
		if (Game.IsControlJustPressed((Control)73) || Game.IsControlJustPressed((Control)44))
		{
			return;
		}
		if (modLemonUIObjectPool != null && modLemonUIObjectPool.AreAnyVisible)
		{
			modLemonUIObjectPool.Process();
		}
		if (Game.IsControlJustPressed((Control)MenuOpenKey))
		{
			MainMenuStuff.Visible = !MainMenuStuff.Visible;
		}
		if (!PlayingCutscene)
		{
			return;
		}
		if ((Entity)(object)CutsenePed2 != (Entity)null)
		{
			((Entity)CutsenePed2).IsVisible = false;
		}
		if ((Entity)(object)CutsenePed3 != (Entity)null)
		{
			((Entity)CutsenePed3).IsVisible = false;
		}
		if ((Entity)(object)CutsenePed4 != (Entity)null)
		{
			((Entity)CutsenePed4).IsVisible = false;
		}
		if (Game.IsControlPressed((Control)22))
		{
			Function.Call((Hash)(-3305433416267445730L));
			Script.Wait(100);
			Function.Call((Hash)(-3454974875737139118L), InputArgument.op_Implicit(CurrentPlayingCutscene));
			PlayingCutscene = false;
			EndCutscene();
			Script.Wait(100);
		}
		if (Function.Call<bool>((Hash)(-3187737643600379106L)))
		{
			Function.Call((Hash)4754298153418524448L, InputArgument.op_Implicit(true));
			Function.Call((Hash)(-9181728881222301740L), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false), InputArgument.op_Implicit(false));
			if (Function.Call<int>((Hash)(-1862873226061817159L)) >= Function.Call<int>((Hash)(-1273479337795616556L)) - 1)
			{
				Function.Call((Hash)(-3305433416267445730L));
				Script.Wait(100);
				Function.Call((Hash)(-3454974875737139118L), InputArgument.op_Implicit(CurrentPlayingCutscene));
				PlayingCutscene = false;
				EndCutscene();
				Script.Wait(100);
			}
			if (!PlayingSPCutscene)
			{
				if (RepalceMp1)
				{
					GetPedOutfitCutscene("MP_1", Game.Player.Character);
				}
				if (RepalceMp1)
				{
					if ((int)Game.Player.Character.Gender == 0)
					{
						GetPedOutfitCutscene("MP_Male_Character", Game.Player.Character);
					}
					else
					{
						GetPedOutfitCutscene("MP_1", Game.Player.Character);
					}
				}
				if (RepalceMp2 && (Entity)(object)NonCutscene2 != (Entity)null)
				{
					GetPedOutfitCutscene("MP_2", NonCutscene2);
				}
				if (RepalceMp3 && (Entity)(object)NonCutscene3 != (Entity)null)
				{
					GetPedOutfitCutscene("MP_3", NonCutscene3);
				}
				if (RepalceMp4 && (Entity)(object)NonCutscene4 != (Entity)null)
				{
					GetPedOutfitCutscene("MP_4", NonCutscene4);
				}
			}
			else
			{
				if (RepalceMp1)
				{
					GetPedOutfitCutscene("Franklin", Game.Player.Character);
				}
				if (RepalceMp2)
				{
					GetPedOutfitCutscene("Michael", Game.Player.Character);
				}
				if (RepalceMp3)
				{
					GetPedOutfitCutscene("Trevor", Game.Player.Character);
				}
			}
		}
		if (!Function.Call<bool>((Hash)(-3187737643600379106L)))
		{
			Function.Call((Hash)(-3305433416267445730L));
			Function.Call((Hash)(-3454974875737139118L), InputArgument.op_Implicit(CurrentPlayingCutscene));
			PlayingCutscene = false;
			EndCutscene();
			Screen.FadeOut(1000);
			Script.Wait(1200);
			Screen.FadeIn(1000);
		}
	}

	private void OnShutdown(object sender, EventArgs e)
	{
	}
}
