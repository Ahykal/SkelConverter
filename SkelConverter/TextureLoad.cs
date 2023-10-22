using Spine;

namespace SkelConverter
{
    internal class TextureLoad : TextureLoader
    {
        public void Load(AtlasPage page, string path)
        {
            return;
        }

        public void Unload(object texture)
        {
            return;
        }

        public static TextureLoad Create()
        {
            return new TextureLoad();
        }
    }
}