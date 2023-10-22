// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Spine;

namespace SkelConverter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string folder;
            List<string> filesExcept = new List<string>();
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
                    SkeletonHappy s;
                    Action dispose;
                    s = SkeletonHappy.FromFile(skelPath, fileName, out dispose);
                    new Worker(s).Work();
                    dispose();

                    SkeletonData skeletonData;
                    TextureLoader textureLoader = TextureLoad.Create();
                    Atlas atlas = new Atlas($"{fileName}.atlas", textureLoader);
                    var sb = new SkeletonBinary(atlas);
                    skeletonData = sb.ReadSkeletonData(skelPath);
                    Dictionary<string, object> jsonObject = SkelDataConverter.FromSkeletonData(skeletonData);
                    string json = JsonConvert.SerializeObject(jsonObject);
                    File.WriteAllText($"{fileName}.json", json);

                    Console.WriteLine($"{fileName}. Export Successfully");
                }
                else
                {
                    filesExcept.Add(fileName);
                }
            }
            fileNames.ForEach(fileName => SkelConvert(fileName));
            filesExcept.ForEach(fileName => Console.WriteLine($"Not Found:{fileName}"));
            Console.WriteLine("Complete");
            Console.ReadKey();
        }
    }
}