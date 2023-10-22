// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Spine;
using System.IO;
using System.Xml.Linq;

namespace SkelConverter
{
    internal class Program
    {
        static void RenameFiles(string path) {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles("*.skel.txt", SearchOption.AllDirectories);
            FileInfo[] atlasFiles = dirInfo.GetFiles("*.atlas.txt", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                string newFileName = file.FullName.Replace(".skel.txt", ".skel");
                file.MoveTo(newFileName);
                
            }

            foreach (FileInfo atlasFile in atlasFiles)
            {
                string newFileName = atlasFile.FullName.Replace(".atlas.txt", ".atlas");
                atlasFile.MoveTo(newFileName);
            }
        }
        private static void Main(string[] args)
        {
            string folder;
            List<string> filesExcept = new List<string>();
            List<string> filesExcept38 = new List<string>();
        setFolder:
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Input or Drag a folder path...");
                    folder = Console.ReadLine() ?? String.Empty;
                }
                else
                {
                    folder = args[0];
                }
            }

            if (!Directory.Exists(folder))
            {
                Console.Write("Please Re ");
                args = new string[] { };
                goto setFolder;
            }

            Console.WriteLine($"Traverse {folder}");

            Console.WriteLine($"Rename skel.txt&json.txt files...");
            RenameFiles(folder);

             var atlasFiles = Directory.GetFiles(folder, "*.atlas", SearchOption.AllDirectories);
            var fileNames = new List<string>();
            foreach (var file in atlasFiles)
            {
                fileNames.Add(file.Substring(0, file.LastIndexOf(".atlas", StringComparison.OrdinalIgnoreCase)));
            }

            void SkelConvert(string fileName)
            {
                var skelPath = fileName + ".skel";
                if (File.Exists(skelPath))
                {
                    try
                    {
                        SkeletonHappy s;
                        Action dispose;
                        s = SkeletonHappy.FromFile(skelPath, fileName, out dispose);
                        new Worker(s).Work();
                        dispose();
                        
                    }
                    catch
                    {
                        filesExcept38.Add(fileName + ".38.skel");
                    }

                    SkeletonData skeletonData;
                    TextureLoader textureLoader = TextureLoad.Create();
                    Atlas atlas = new Atlas($"{fileName}.atlas", textureLoader);
                    var sb = new SkeletonBinary(atlas);
                    skeletonData = sb.ReadSkeletonData(skelPath);
                    Dictionary<string, object> jsonObject = SkelDataConverter.FromSkeletonData(skeletonData);
                    string json = JsonConvert.SerializeObject(jsonObject);
                    File.WriteAllText($"{fileName}.json", json);

                    Console.WriteLine($"{fileName}.38.skel&json Export Successfully");
                }
                else
                {
                    filesExcept.Add(fileName);
                }
            }
            fileNames.ForEach(fileName => SkelConvert(fileName));
            filesExcept38.ForEach(fileName => { 
                File.Delete(fileName);
                Console.WriteLine($"Skip:{fileName}");
            });
            filesExcept.ForEach(fileName => Console.WriteLine($"Not Found:{fileName}"));
            
            Console.WriteLine("Complete");
            Console.ReadKey();
        }
    }
}