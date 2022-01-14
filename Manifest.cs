using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "SEO",
    Author = "Etch",
    Website = "https://etchuk.com",
    Version = "0.7.5"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.SEO.Redirects",
    Name = "Redirects",
    Description = "Create 301 redirects.",
    Dependencies = new[]
    {
        "OrchardCore.Title"
    },
    Category = "Content"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.SEO.Redirects.Import",
    Name = "Import Redirects",
    Description = "Import 301 redirects from a spreadsheet.",
    Dependencies = new[]
    {
        "Etch.OrchardCore.SEO.Redirects"
    },
    Category = "Content"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.SEO.RobotsTxt",
    Name = "Robots.txt",
    Description = "Manage contents of robots.txt.",
    Category = "Content"
)]


[assembly: Feature(
    Id = "Etch.OrchardCore.SEO.HostnameRedirects",
    Name = "Hostname Redirects",
    Description = "Manage hostname redirects.",
    Category = "Content"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.SEO.MetaTags",
    Name = "Meta Tags",
    Dependencies = new[]
    {
        "Etch.OrchardCore.Fields.Dictionary",
        "OrchardCore.Media"
    },
    Description = "Manage meta tags for content items.",
    Category = "Content"
)]
