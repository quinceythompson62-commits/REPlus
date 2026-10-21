// ClothBuilder - builds a singleplayer add-on clothing DLC (dlc.rpf) for
// GTA V Enhanced from legacy or gen9 clothing drawables/textures.
// The DLC layout and YMT generation follow grzyClothTool's singleplayer build.
//
// Commands:
//   inspect <file-or-folder>          print info about ydd/ytd/ymt/rpf files
//   extract <rpf> <outFolder>         extract an (OPEN) rpf recursively
//   convert <inFolder> <outFolder>    convert legacy ydd/ytd/ydr/yft to gen9
//   build   <spec.txt> <outFolder>    build <outFolder>\<dlc>\dlc.rpf from a spec
//
// Spec (tab separated, one item per line, '#' comments):
//   dlc   <name>
//   comp  <compIndex 0-11> <ydd> <skin 0|1> <audio> <label> <tex1|tex2|...>
//   prop  <anchor 0-12>    <ydd> <audio> <label> <tex1|tex2|...>
using CodeWalker.Core.Utils;
using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClothBuilder
{
    class Item
    {
        public bool IsProp;
        public int Type;          // component index or prop anchor
        public string Ydd;
        public bool Skin;
        public string Audio = "none";
        public string Label;
        public List<string> Textures = new List<string>();
        public int Number;        // local drawable index within its type
    }

    static class Program
    {
        static readonly string[] CompNames = { "head", "berd", "hair", "uppr", "lowr", "hand", "feet", "teef", "accs", "task", "decl", "jbib" };
        static readonly string[] PropNames = { "p_head", "p_eyes", "p_ears", "p_mouth", "p_lhand", "p_rhand", "p_lwrist", "p_rwrist", "p_hip", "p_lfoot", "p_rfoot", "p_ph_l_hand", "p_ph_r_hand" };
        static readonly string[] CompNice = { "Head", "Masks", "Hair", "Arms/Torso", "Legs", "Bags", "Shoes", "Accessories(teef)", "Undershirt", "Body armor", "Decals", "Tops" };
        static readonly string[] PropNice = { "Hats", "Glasses", "Ears", "Mouth", "L hand", "R hand", "Watches", "Bracelets", "Hip", "L foot", "R foot", "", "" };

        static int Main(string[] args)
        {
            RpfManager.IsGen9 = true;
            if (args.Length < 2) { Usage(); return 1; }
            try
            {
                switch (args[0].ToLowerInvariant())
                {
                    case "inspect": Inspect(args[1]); return 0;
                    case "extract": if (args.Length < 3) break; Extract(args[1], args[2]); return 0;
                    case "convert": if (args.Length < 3) break; ConvertFolder(args[1], args[2]); return 0;
                    case "build": if (args.Length < 3) break; return Build(args[1], args[2]);
                    case "overlay": if (args.Length < 3) break; return Overlay(args[1], args[2]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                return 2;
            }
            Usage();
            return 1;
        }

        static void Usage()
        {
            Console.WriteLine("ClothBuilder inspect <file|folder> | extract <rpf> <out> | convert <in> <out> | build <spec.txt> <out>");
        }

        // ---------------------------------------------------------------- inspect
        static void Inspect(string path)
        {
            IEnumerable<string> files = Directory.Exists(path)
                ? Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                : new[] { path };
            foreach (var f in files)
            {
                var ext = Path.GetExtension(f).ToLowerInvariant();
                if (ext != ".ydd" && ext != ".ytd" && ext != ".ymt" && ext != ".rpf" && ext != ".ydr" && ext != ".yft" && ext != ".yld") continue;
                try { InspectFile(f, File.ReadAllBytes(f), ext); }
                catch (Exception ex) { Console.WriteLine($"{f}\tERROR {ex.GetType().Name}: {ex.Message}"); }
            }
        }

        static void InspectFile(string name, byte[] data, string ext)
        {
            switch (ext)
            {
                case ".ydd":
                    {
                        var y = new YddFile(); y.Load(data);
                        var ver = (y.RpfFileEntry as RpfResourceFileEntry)?.Version;
                        var sb = new StringBuilder();
                        sb.Append($"{name}\tYDD v{ver}\tdrawables={y.Drawables?.Length ?? 0}");
                        foreach (var d in y.Drawables ?? new Drawable[0])
                        {
                            var shaders = d.ShaderGroup?.Shaders?.data_items?.Select(s => s.Name.ToString()).ToArray() ?? new string[0];
                            var emb = d.ShaderGroup?.TextureDictionary?.Textures?.data_items?.Length ?? 0;
                            sb.Append($"\t[{d.Name} shaders={string.Join(",", shaders)} embTex={emb}]");
                        }
                        Console.WriteLine(sb.ToString());
                        break;
                    }
                case ".ytd":
                    {
                        var y = new YtdFile(); y.Load(data);
                        var ver = (y.RpfFileEntry as RpfResourceFileEntry)?.Version;
                        var tex = y.TextureDict?.Textures?.data_items ?? new Texture[0];
                        Console.WriteLine($"{name}\tYTD v{ver}\t" + string.Join(" ", tex.Select(t => $"{t.Name}:{t.Width}x{t.Height}:{t.Format}")));
                        break;
                    }
                case ".ydr":
                case ".yft":
                case ".yld":
                    {
                        var rfe = RpfFile.CreateResourceFileEntry(ref data, 0);
                        Console.WriteLine($"{name}\t{ext.ToUpperInvariant().TrimStart('.')} v{rfe?.Version}");
                        break;
                    }
                case ".ymt":
                    {
                        var y = new PedFile(); RpfFile.LoadResourceFile(y, data, 2);
                        var vi = y.VariationInfo;
                        if (vi == null) { Console.WriteLine($"{name}\tYMT (not a ped variation ymt)"); break; }
                        var sb = new StringBuilder($"{name}\tPED YMT");
                        var comps = vi.ComponentData3;
                        if (comps != null)
                            foreach (var c in comps)
                            {
                                if (c == null) continue;
                                var n = c.DrawblData3?.Length ?? 0;
                                var tex = string.Join(",", (c.DrawblData3 ?? new MCPVDrawblData[0]).Select(d => (d.TexData?.Length ?? 0).ToString()));
                                sb.Append($"\t{CompNames[c.ComponentType]}:{n}[{tex}]");
                            }
                        var props = vi.PropInfo?.PropMetaData;
                        if (props != null)
                            foreach (var g in props.GroupBy(p => p.Data.anchorId))
                                sb.Append($"\t{PropNames[g.Key]}:{g.Count()}[{string.Join(",", g.Select(p => (p.TexData?.Length ?? 0).ToString()))}]");
                        Console.WriteLine(sb.ToString());
                        break;
                    }
                case ".rpf":
                    {
                        Console.WriteLine($"{name}\tRPF");
                        var tmp = Path.Combine(Path.GetTempPath(), "cb_" + Guid.NewGuid().ToString("N"));
                        Extract(name, tmp, quiet: true);
                        foreach (var f in Directory.EnumerateFiles(tmp, "*", SearchOption.AllDirectories))
                        {
                            var e = Path.GetExtension(f).ToLowerInvariant();
                            var rel = f.Substring(tmp.Length + 1);
                            if (e == ".ydd" || e == ".ytd" || e == ".ymt" || e == ".ydr" || e == ".yft")
                            {
                                try { InspectFile(name + "|" + rel, File.ReadAllBytes(f), e); }
                                catch (Exception ex) { Console.WriteLine($"{name}|{rel}\tERROR {ex.Message}"); }
                            }
                            else Console.WriteLine($"{name}|{rel}\t{new FileInfo(f).Length}");
                        }
                        try { Directory.Delete(tmp, true); } catch { }
                        break;
                    }
            }
        }

        // ---------------------------------------------------------------- extract
        static void Extract(string rpfPath, string outDir, bool quiet = false)
        {
            var rpf = new RpfFile(Path.GetFullPath(rpfPath), Path.GetFileName(rpfPath));
            var errors = new List<string>();
            rpf.ScanStructure(null, e => errors.Add(e));
            foreach (var e in errors) Console.WriteLine("scan: " + e);
            ExtractRpf(rpf, outDir, quiet);
        }

        static void ExtractRpf(RpfFile rpf, string outDir, bool quiet)
        {
            Directory.CreateDirectory(outDir);
            if (rpf.Encryption == RpfEncryption.NG)
                Console.WriteLine($"WARNING {rpf.Path}: NG-encrypted archive, contents can't be read without game keys");
            foreach (var entry in rpf.AllEntries)
            {
                if (!(entry is RpfFileEntry fe)) continue;
                if (fe.NameLower.EndsWith(".rpf")) continue; // children handled below
                var rel = RelPath(rpf, entry);
                var dest = Path.Combine(outDir, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                var bytes = rpf.ExtractFile(fe);
                if (bytes == null) { Console.WriteLine($"FAILED {rel}: {rpf.LastError}"); continue; }
                File.WriteAllBytes(dest, bytes);
                if (!quiet) Console.WriteLine($"{rel}\t{bytes.Length}");
            }
            if (rpf.Children != null)
                foreach (var child in rpf.Children)
                {
                    var rel = RelPath(rpf, child.ParentFileEntry);
                    ExtractRpf(child, Path.Combine(outDir, rel), quiet);
                }
        }

        static string RelPath(RpfFile rpf, RpfEntry entry)
        {
            // entry.Path is "<rpf path>\dir\file"; strip the archive's own path
            var p = entry.Path ?? entry.Name;
            var root = rpf.Path ?? "";
            if (p.StartsWith(root, StringComparison.OrdinalIgnoreCase)) p = p.Substring(root.Length);
            return p.TrimStart('\\', '/').Replace('/', '\\');
        }

        // ---------------------------------------------------------------- convert
        static byte[] ToGen9(string path, out bool converted)
        {
            var data = File.ReadAllBytes(path);
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var outData = Gen9Converter.TryConvert(data, ext, s => { }, path, true, out converted);
            if (outData == null) throw new Exception("conversion failed for " + path);
            return outData;
        }

        static void ConvertFolder(string inDir, string outDir)
        {
            inDir = Path.GetFullPath(inDir);
            foreach (var f in Directory.EnumerateFiles(inDir, "*", SearchOption.AllDirectories))
            {
                var rel = f.Substring(inDir.Length).TrimStart('\\', '/');
                var dest = Path.Combine(outDir, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                try
                {
                    var bytes = ToGen9(f, out var conv);
                    File.WriteAllBytes(dest, bytes);
                    Console.WriteLine($"{rel}\t{(conv ? "converted" : "copied")}");
                }
                catch (Exception ex) { Console.WriteLine($"{rel}\tERROR {ex.Message}"); }
            }
        }


        // ---------------------------------------------------------------- overlay
        // Builds a DLC whose RPFs override existing game files by name (like Rockstar's own
        // mppatches): face textures, player_zero/one/two remasters, tattoo overlays.
        // Spec (tab separated): dlc <name> | file <rpf name> <path inside rpf> <source file>
        static int Overlay(string specPath, string outRoot)
        {
            string dlc = null;
            var baseDir = Path.GetDirectoryName(Path.GetFullPath(specPath));
            var files = new List<(string rpf, string inner, string src)>();
            foreach (var raw in File.ReadAllLines(specPath))
            {
                var line = raw.TrimEnd('\r');
                if (line.Trim().Length == 0 || line.TrimStart().StartsWith("#")) continue;
                var c = line.Split('\t');
                if (c[0] == "dlc") dlc = c[1].Trim().ToLowerInvariant();
                else if (c[0] == "file") files.Add((c[1], c[2].Replace('/', '\\'), Path.IsPathRooted(c[3]) ? c[3] : Path.Combine(baseDir, c[3])));
                else throw new Exception("unknown spec line: " + line);
            }
            if (dlc == null) throw new Exception("spec has no 'dlc' line");
            var outDir = Path.Combine(Path.GetFullPath(outRoot), dlc);
            Directory.CreateDirectory(outDir);
            var dlcRpfPath = Path.Combine(outDir, "dlc.rpf");
            if (File.Exists(dlcRpfPath)) File.Delete(dlcRpfPath);

            var rpfNames = files.Select(f => f.rpf).Distinct().ToList();
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<CDataFileMgr__ContentsOfDataFileXml>");
            sb.AppendLine("  <disabledFiles />");
            sb.AppendLine("  <includedXmlFiles />");
            sb.AppendLine("  <includedDataFiles />");
            sb.AppendLine("  <dataFiles>");
            foreach (var r in rpfNames)
            {
                sb.AppendLine("    <Item>");
                sb.AppendLine($"      <filename>dlc_{dlc}:/%PLATFORM%/models/cdimages/{r}</filename>");
                sb.AppendLine("      <fileType>RPF_FILE</fileType>");
                sb.AppendLine("      <overlay value=\"true\" />");
                sb.AppendLine("      <disabled value=\"true\" />");
                sb.AppendLine("      <persistent value=\"true\" />");
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </dataFiles>");
            sb.AppendLine("  <contentChangeSets>");
            sb.AppendLine("    <Item>");
            sb.AppendLine($"      <changeSetName>{dlc.ToUpperInvariant()}_GEN</changeSetName>");
            sb.AppendLine("      <mapChangeSetData />");
            sb.AppendLine("      <filesToInvalidate />");
            sb.AppendLine("      <filesToDisable />");
            sb.AppendLine("      <filesToEnable>");
            foreach (var r in rpfNames) sb.AppendLine($"        <Item>dlc_{dlc}:/%PLATFORM%/models/cdimages/{r}</Item>");
            sb.AppendLine("      </filesToEnable>");
            sb.AppendLine("      <txdToLoad />");
            sb.AppendLine("      <txdToUnload />");
            sb.AppendLine("      <residentResources />");
            sb.AppendLine("      <unregisterResources />");
            sb.AppendLine("      <requiresLoadingScreen value=\"false\" />");
            sb.AppendLine("    </Item>");
            sb.AppendLine("  </contentChangeSets>");
            sb.AppendLine("  <patchFiles />");
            sb.AppendLine("</CDataFileMgr__ContentsOfDataFileXml>");

            var root = RpfFile.CreateNew(outDir, "dlc.rpf", RpfEncryption.OPEN);
            RpfFile.CreateFile(root.Root, "content.xml", Encoding.UTF8.GetBytes(sb.ToString()));
            RpfFile.CreateFile(root.Root, "setup2.xml", Encoding.UTF8.GetBytes(SetupXml(dlc)));
            var x64 = RpfFile.CreateDirectory(root.Root, "x64");
            var models = RpfFile.CreateDirectory(x64, "models");
            var cd = RpfFile.CreateDirectory(models, "cdimages");
            int n = 0;
            foreach (var r in rpfNames)
            {
                var sub = RpfFile.CreateNew(cd, r);
                var dirs = new Dictionary<string, RpfDirectoryEntry>(StringComparer.OrdinalIgnoreCase) { [""] = sub.Root };
                foreach (var f in files.Where(x => x.rpf == r))
                {
                    var dir = Path.GetDirectoryName(f.inner) ?? "";
                    if (!dirs.TryGetValue(dir, out var de))
                    {
                        de = sub.Root;
                        var acc = "";
                        foreach (var part in dir.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            acc = acc.Length == 0 ? part : acc + "\\" + part;
                            if (!dirs.TryGetValue(acc, out var next)) { next = RpfFile.CreateDirectory(de, part); dirs[acc] = next; }
                            de = next;
                        }
                    }
                    try
                    {
                        var bytes = ToGen9(f.src, out var conv);
                        RpfFile.CreateFile(de, Path.GetFileName(f.inner), bytes);
                        Console.WriteLine($"{r}|{f.inner}\t{(conv ? "converted" : "as is")}");
                        n++;
                    }
                    catch (Exception ex) { Console.WriteLine($"SKIP {f.inner}: {ex.Message}"); }
                }
            }
            Console.WriteLine($"BUILT {dlcRpfPath}  files={n}");
            return 0;
        }

        // ---------------------------------------------------------------- build
        static int Build(string specPath, string outRoot)
        {
            string dlc = null;
            var items = new List<Item>();
            var baseDir = Path.GetDirectoryName(Path.GetFullPath(specPath));
            int lineNo = 0;
            foreach (var raw in File.ReadAllLines(specPath))
            {
                lineNo++;
                var line = raw.TrimEnd('\r');
                if (line.Trim().Length == 0 || line.TrimStart().StartsWith("#")) continue;
                var c = line.Split('\t');
                Func<string, string> P = s => Path.IsPathRooted(s) ? s : Path.Combine(baseDir, s);
                switch (c[0])
                {
                    case "dlc": dlc = c[1].Trim().ToLowerInvariant(); break;
                    case "comp":
                        items.Add(new Item
                        {
                            IsProp = false, Type = int.Parse(c[1]), Ydd = P(c[2]), Skin = c[3] == "1",
                            Audio = c[4], Label = c[5],
                            Textures = c[6].Split('|').Where(x => x.Length > 0).Select(P).ToList()
                        });
                        break;
                    case "prop":
                        items.Add(new Item
                        {
                            IsProp = true, Type = int.Parse(c[1]), Ydd = P(c[2]),
                            Audio = c[3], Label = c[4],
                            Textures = c[5].Split('|').Where(x => x.Length > 0).Select(P).ToList()
                        });
                        break;
                    default: throw new Exception($"spec line {lineNo}: unknown '{c[0]}'");
                }
            }
            if (dlc == null) throw new Exception("spec has no 'dlc' line");

            // validate inputs: every ydd/ytd must load, otherwise drop the item
            var good = new List<Item>();
            var cache = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
            foreach (var it in items)
            {
                try
                {
                    if (it.Textures.Count > 26) it.Textures = it.Textures.Take(26).ToList();
                    cache[it.Ydd] = ToGen9(it.Ydd, out _);
                    if (it.Textures.Count == 0)
                    {
                        // drawable carries its textures embedded: make a ytd out of them
                        var y = new YddFile(); y.Load(cache[it.Ydd]);
                        var emb = y.Drawables?.SelectMany(d => d.ShaderGroup?.TextureDictionary?.Textures?.data_items ?? new Texture[0]).ToList();
                        if (emb == null || emb.Count == 0) throw new Exception("no textures");
                        var td = new TextureDictionary();
                        td.BuildFromTextureList(emb);
                        var ytd = new YtdFile { TextureDict = td };
                        var key = it.Ydd + "#embedded.ytd";
                        cache[key] = ytd.Save();
                        it.Textures.Add(key);
                        Console.WriteLine($"{it.Label}: built ytd from {emb.Count} embedded textures");
                    }
                    foreach (var t in it.Textures) if (!cache.ContainsKey(t)) cache[t] = ToGen9(t, out _);
                    good.Add(it);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SKIP {it.Label}: {ex.Message}");
                }
            }
            items = good;

            // number drawables per type
            foreach (var g in items.GroupBy(i => (i.IsProp, i.Type)))
            {
                int n = 0;
                foreach (var it in g)
                {
                    if (n > 127) throw new Exception($"too many drawables for {(it.IsProp ? PropNames[it.Type] : CompNames[it.Type])} (max 128)");
                    it.Number = n++;
                }
            }
            var comps = items.Where(i => !i.IsProp).OrderBy(i => i.Type).ThenBy(i => i.Number).ToList();
            var props = items.Where(i => i.IsProp).OrderBy(i => i.Type).ThenBy(i => i.Number).ToList();

            var outDir = Path.Combine(Path.GetFullPath(outRoot), dlc);
            Directory.CreateDirectory(outDir);
            var dlcRpfPath = Path.Combine(outDir, "dlc.rpf");
            if (File.Exists(dlcRpfPath)) File.Delete(dlcRpfPath);

            const string ped = "mp_m_freemode_01";
            var dlcRpf = RpfFile.CreateNew(outDir, "dlc.rpf", RpfEncryption.OPEN);
            RpfFile.CreateFile(dlcRpf.Root, "content.xml", Encoding.UTF8.GetBytes(ContentXml(dlc, props.Count > 0)));
            RpfFile.CreateFile(dlcRpf.Root, "setup2.xml", Encoding.UTF8.GetBytes(SetupXml(dlc)));
            var x64 = RpfFile.CreateDirectory(dlcRpf.Root, "x64");
            var common = RpfFile.CreateDirectory(dlcRpf.Root, "common");
            var data = RpfFile.CreateDirectory(common, "data");
            var models = RpfFile.CreateDirectory(x64, "models");
            var cdimages = RpfFile.CreateDirectory(models, "cdimages");

            RpfFile.CreateFile(data, $"{ped}_{dlc}.meta", Encoding.UTF8.GetBytes(ShopMeta(ped, dlc)));

            var compRpf = RpfFile.CreateNew(cdimages, dlc + "_male.rpf");
            var compFolder = RpfFile.CreateDirectory(compRpf.Root, $"{ped}_{dlc}");
            RpfFile.CreateFile(compRpf.Root, $"{ped}_{dlc}.ymt", BuildYmt(dlc, comps, props));

            RpfDirectoryEntry propFolder = null;
            if (props.Count > 0)
            {
                var propRpf = RpfFile.CreateNew(cdimages, dlc + "_male_p.rpf");
                propFolder = RpfFile.CreateDirectory(propRpf.Root, $"{ped}_p_{dlc}");
            }

            var list = new StringBuilder();
            list.AppendLine($"Add-on clothing pack '{dlc}' for MP Male (mp_m_freemode_01)");
            list.AppendLine("Add-on items are listed AFTER all the normal GTA items in each category.");
            list.AppendLine("Number below = position among this pack's items in that category (0 = first add-on item).");
            list.AppendLine();
            foreach (var it in comps.Concat(props))
            {
                var typeName = it.IsProp ? PropNames[it.Type] : CompNames[it.Type];
                var num = it.Number.ToString("D3");
                var folder = it.IsProp ? propFolder : compFolder;
                var yddName = it.IsProp ? $"{typeName}_{num}.ydd" : $"{typeName}_{num}_{(it.Skin ? "r" : "u")}.ydd";
                RpfFile.CreateFile(folder, yddName, cache[it.Ydd]);
                for (int t = 0; t < it.Textures.Count; t++)
                {
                    char letter = (char)('a' + t);
                    var texName = it.IsProp ? $"{typeName}_diff_{num}_{letter}.ytd" : $"{typeName}_diff_{num}_{letter}_{(it.Skin ? "whi" : "uni")}.ytd";
                    RpfFile.CreateFile(folder, texName, cache[it.Textures[t]]);
                }
                var cat = it.IsProp ? PropNice[it.Type] : CompNice[it.Type];
                list.AppendLine($"{cat,-18} #{it.Number,-3} {it.Label}  ({it.Textures.Count} colour{(it.Textures.Count == 1 ? "" : "s")})");
            }
            File.WriteAllText(Path.Combine(outRoot, dlc + "_items.txt"), list.ToString());
            Console.WriteLine($"BUILT {dlcRpfPath}  components={comps.Count} props={props.Count}");
            return 0;
        }

        static string ContentXml(string p, bool hasProps)
        {
            var files = new List<(string name, string type, bool persistent)>
            {
                ($"dlc_{p}:/common/data/mp_m_freemode_01_{p}.meta", "SHOP_PED_APPAREL_META_FILE", false),
                ($"dlc_{p}:/%PLATFORM%/models/cdimages/{p}_male.rpf", "RPF_FILE", true),
            };
            if (hasProps) files.Add(($"dlc_{p}:/%PLATFORM%/models/cdimages/{p}_male_p.rpf", "RPF_FILE", true));
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<CDataFileMgr__ContentsOfDataFileXml>");
            sb.AppendLine("  <disabledFiles />");
            sb.AppendLine("  <includedXmlFiles />");
            sb.AppendLine("  <includedDataFiles />");
            sb.AppendLine("  <dataFiles>");
            foreach (var f in files)
            {
                sb.AppendLine("    <Item>");
                sb.AppendLine($"      <filename>{f.name}</filename>");
                sb.AppendLine($"      <fileType>{f.type}</fileType>");
                sb.AppendLine("      <overlay value=\"false\" />");
                sb.AppendLine("      <disabled value=\"true\" />");
                sb.AppendLine($"      <persistent value=\"{(f.persistent ? "true" : "false")}\" />");
                sb.AppendLine("    </Item>");
            }
            sb.AppendLine("  </dataFiles>");
            sb.AppendLine("  <contentChangeSets>");
            sb.AppendLine("    <Item>");
            sb.AppendLine($"      <changeSetName>{p.ToUpperInvariant()}_GEN</changeSetName>");
            sb.AppendLine("      <mapChangeSetData />");
            sb.AppendLine("      <filesToInvalidate />");
            sb.AppendLine("      <filesToDisable />");
            sb.AppendLine("      <filesToEnable>");
            foreach (var f in files) sb.AppendLine($"        <Item>{f.name}</Item>");
            sb.AppendLine("      </filesToEnable>");
            sb.AppendLine("      <txdToLoad />");
            sb.AppendLine("      <txdToUnload />");
            sb.AppendLine("      <residentResources />");
            sb.AppendLine("      <unregisterResources />");
            sb.AppendLine("      <requiresLoadingScreen value=\"false\" />");
            sb.AppendLine("    </Item>");
            sb.AppendLine("  </contentChangeSets>");
            sb.AppendLine("  <patchFiles />");
            sb.AppendLine("</CDataFileMgr__ContentsOfDataFileXml>");
            return sb.ToString();
        }

        static string SetupXml(string p)
        {
            var ts = DateTimeOffset.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            return
$@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SSetupData>
  <deviceName>dlc_{p}</deviceName>
  <datFile>content.xml</datFile>
  <timeStamp>{ts}</timeStamp>
  <nameHash>{p}</nameHash>
  <contentChangeSets />
  <contentChangeSetGroups>
    <Item>
      <NameHash>GROUP_STARTUP</NameHash>
      <ContentChangeSets>
        <Item>{p.ToUpperInvariant()}_GEN</Item>
      </ContentChangeSets>
    </Item>
  </contentChangeSetGroups>
  <startupScript />
  <scriptCallstackSize value=""0"" />
  <type>EXTRACONTENT_COMPAT_PACK</type>
  <order value=""999"" />
  <minorOrder value=""0"" />
  <isLevelPack value=""false"" />
  <dependencyPackHash />
  <requiredVersion />
  <subPackCount value=""0"" />
</SSetupData>
";
        }

        static string ShopMeta(string ped, string p)
        {
            return
$@"<?xml version=""1.0"" encoding=""UTF-8""?>
<ShopPedApparel>
    <pedName>{ped}</pedName>
    <dlcName>{p}</dlcName>
    <fullDlcName>{ped}_{p}</fullDlcName>
    <eCharacter>SCR_CHAR_MULTIPLAYER</eCharacter>
    <creatureMetaData>mp_creaturemetadata_m_{p}</creatureMetaData>
    <pedOutfits>
    </pedOutfits>
    <pedComponents>
    </pedComponents>
    <pedProps>
    </pedProps>
</ShopPedApparel>
";
        }

        static byte[] BuildYmt(string dlc, List<Item> comps, List<Item> props)
        {
            var mb = new MetaBuilder();
            mb.EnsureBlock(MetaName.CPedVariationInfo);
            var ped = new CPedVariationInfo
            {
                bHasDrawblVariations = 1,
                bHasTexVariations = 1,
                bHasLowLODs = 0,
                bIsSuperLOD = 0
            };

            var avail = Enumerable.Repeat((byte)255, 12).ToArray();
            byte cnt = 0;
            for (int i = 0; i < 12; i++) if (comps.Any(c => c.Type == i)) avail[i] = cnt++;
            ped.availComp = new ArrayOfBytes12
            {
                b00 = avail[0], b01 = avail[1], b02 = avail[2], b03 = avail[3], b04 = avail[4], b05 = avail[5],
                b06 = avail[6], b07 = avail[7], b08 = avail[8], b09 = avail[9], b10 = avail[10], b11 = avail[11]
            };

            var compData = new List<CPVComponentData>();
            for (int i = 0; i < 12; i++)
            {
                if (avail[i] == 255) continue;
                var arr = comps.Where(c => c.Type == i).ToArray();
                var drawables = new CPVDrawblData[arr.Length];
                for (int d = 0; d < arr.Length; d++)
                {
                    var tex = new CPVTextureData[arr[d].Textures.Count];
                    for (int t = 0; t < tex.Length; t++)
                    {
                        tex[t].texId = (byte)(arr[d].Skin ? 1 : 0);
                        tex[t].distribution = 255;
                    }
                    drawables[d].propMask = (byte)(arr[d].Skin ? 17 : 1);
                    drawables[d].numAlternatives = 0;
                    drawables[d].clothData = new CPVDrawblData__CPVClothComponentData { ownsCloth = 0 };
                    drawables[d].aTexData = mb.AddItemArrayPtr(MetaName.CPVTextureData, tex);
                }
                compData.Add(new CPVComponentData
                {
                    numAvailTex = (byte)Math.Min(255, arr.Sum(x => x.Textures.Count)),
                    aDrawblData3 = mb.AddItemArrayPtr(MetaName.CPVDrawblData, drawables)
                });
            }
            ped.aComponentData3 = mb.AddItemArrayPtr(MetaName.CPVComponentData, compData.ToArray());

            var infos = new CComponentInfo[comps.Count];
            for (int i = 0; i < infos.Length; i++)
            {
                var c = comps[i];
                infos[i].pedXml_audioID = JenkHash.GenHash(string.IsNullOrEmpty(c.Audio) ? "none" : c.Audio);
                infos[i].pedXml_audioID2 = JenkHash.GenHash("none");
                infos[i].pedXml_expressionMods = new ArrayOfFloats5();
                infos[i].flags = 0;
                infos[i].inclusions = 0;
                infos[i].exclusions = 0;
                infos[i].pedXml_vfxComps = ePedVarComp.PV_COMP_HEAD;
                infos[i].pedXml_flags = 0;
                infos[i].pedXml_compIdx = (byte)c.Type;
                infos[i].pedXml_drawblIdx = (byte)c.Number;
            }
            ped.compInfos = mb.AddItemArrayPtr(MetaName.CComponentInfo, infos);

            var propInfo = new CPedPropInfo { numAvailProps = (byte)props.Count };
            var pm = new CPedPropMetaData[props.Count];
            for (int i = 0; i < pm.Length; i++)
            {
                var p = props[i];
                var tex = new CPedPropTexData[p.Textures.Count];
                for (int t = 0; t < tex.Length; t++)
                {
                    tex[t].inclusions = 0;
                    tex[t].exclusions = 0;
                    tex[t].texId = (byte)t;
                    tex[t].inclusionId = 0;
                    tex[t].exclusionId = 0;
                    tex[t].distribution = 255;
                }
                pm[i].audioId = JenkHash.GenHash(string.IsNullOrEmpty(p.Audio) ? "none" : p.Audio);
                pm[i].expressionMods = new ArrayOfBytes5();
                pm[i].texData = mb.AddItemArrayPtr(MetaName.CPedPropTexData, tex);
                pm[i].renderFlags = 0;
                pm[i].propFlags = 0;
                pm[i].flags = 0;
                pm[i].anchorId = (byte)p.Type;
                pm[i].propId = (byte)p.Number;
                pm[i].stickyness = 0;
            }
            propInfo.aPropMetaData = mb.AddItemArrayPtr(MetaName.CPedPropMetaData, pm);

            var anchorTypes = props.Select(p => p.Type).Distinct().ToArray();
            var anchors = new CAnchorProps[anchorTypes.Length];
            for (int i = 0; i < anchors.Length; i++)
            {
                var ofType = props.Where(p => p.Type == anchorTypes[i]).OrderBy(p => p.Number);
                anchors[i].props = mb.AddByteArrayPtr(ofType.Select(p => (byte)p.Textures.Count).ToArray());
                anchors[i].anchor = (eAnchorPoints)anchorTypes[i];
            }
            propInfo.aAnchors = mb.AddItemArrayPtr(MetaName.CAnchorProps, anchors);
            ped.propInfo = propInfo;
            ped.dlcName = JenkHash.GenHash(dlc);

            mb.AddItem(MetaName.CPedVariationInfo, ped);
            mb.AddStructureInfo(MetaName.CPedVariationInfo);
            mb.AddStructureInfo(MetaName.CPedPropInfo);
            mb.AddStructureInfo(MetaName.CPedPropTexData);
            mb.AddStructureInfo(MetaName.CAnchorProps);
            mb.AddStructureInfo(MetaName.CComponentInfo);
            mb.AddStructureInfo(MetaName.CPVComponentData);
            mb.AddStructureInfo(MetaName.CPVDrawblData);
            mb.AddStructureInfo(MetaName.CPVDrawblData__CPVClothComponentData);
            mb.AddStructureInfo(MetaName.CPVTextureData);
            mb.AddStructureInfo(MetaName.CPedPropMetaData);
            mb.AddEnumInfo(MetaName.ePedVarComp);
            mb.AddEnumInfo(MetaName.eAnchorPoints);
            mb.AddEnumInfo(MetaName.ePropRenderFlags);

            var meta = mb.GetMeta();
            meta.Name = dlc;
            return ResourceBuilder.Build(meta, 2);
        }
    }
}
