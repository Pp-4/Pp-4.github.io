using System.IO;
using System;
namespace sitebuilder
{
    class Program
    {
        const string PAGES_PATH = "pages";
        const string ASSETS_PATH = "assets";
        const string TEMPLATE_PATH = "templates";
        const string OUTPUT_PATTH = "output";
        static void Main(string[] args)
        {
            Console.WriteLine("Building static site...");

            // Find template file
            var templateLocation = Path.Combine(TEMPLATE_PATH, "template.html");
            if (!File.Exists(templateLocation))
            {
                Console.WriteLine($"Missing template file at {Directory.GetCurrentDirectory}/{templateLocation}.");
                return;
            }
            var template = File.ReadAllText(templateLocation);

            // Clean output folder
            if (Directory.Exists(OUTPUT_PATTH)) Directory.Delete(OUTPUT_PATTH, recursive: true);
            Directory.CreateDirectory(OUTPUT_PATTH);

            // Copy assets
            var assetTarget = Path.Combine(OUTPUT_PATTH, ASSETS_PATH);
            if (Directory.Exists(ASSETS_PATH)) CopyDirectory(ASSETS_PATH, assetTarget);

            // Build each HTML file
            BuildPages(template);

            Console.WriteLine("Build complete.");
            return;
        }
        static void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var targetFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, targetFile);
            }
            foreach (var directtory in Directory.GetDirectories(sourceDir))
            {
                CopyDirectory(directtory, targetDir);
            }
        }
        static void BuildPages(string template)
        {
            Directory.CreateDirectory(Path.Combine(OUTPUT_PATTH, "pages"));
            foreach (var file in Directory.GetFiles(PAGES_PATH, "*.html"))
            {
                var filename = Path.GetFileName(file);
                var content = File.ReadAllText(file);
                if (filename == "index.html")
                {
                    var fullPage = template.Replace("{{content}}", content);
                    File.WriteAllText(Path.Combine(OUTPUT_PATTH, filename), fullPage);
                }
                else
                {
                    var fullPage = template
                        .Replace("assets/style.css", "../assets/style.css")
                        .Replace("assets/style.js", "../assets/style.js")
                        .Replace("assets/router.js", "../assets/router.js")
                        .Replace("{{content}}", content);
                    File.WriteAllText(Path.Combine(OUTPUT_PATTH, "pages", filename), fullPage);
                }
                Console.WriteLine($"Built: {filename}");
            }
        }
    }
}