using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlexGridExplorer
{
    public class Song
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public long Duration { get; set; }  // in milliseconds
        public long Size { get; set; }      // in bytes
        public int Rating { get; set; }     // from 0 to 5
    }

    public class MediaLibrary
    {
        public static async Task<List<Song>> LoadAsync()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resName = asm.GetManifestResourceNames().First(name => name.EndsWith("data.zip"));
            var zip = new ZipArchive(asm.GetManifestResourceStream(resName));
            using (var stream = zip.Entries.First(e => e.Name == "songs.json").Open())
            {
                return await JsonSerializer.DeserializeAsync<List<Song>>(stream);
            }
        }
    }
}
