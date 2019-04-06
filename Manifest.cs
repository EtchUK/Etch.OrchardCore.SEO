using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "SEO",
    Author = "Etch",
    Website = "https://etchuk.com",
    Version = "0.3.2"
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
    Id = "Moov2.OrchardCore.SEO.MetaTags",
    Name = "Meta Tags",
    Dependencies = new[]
    {
        "OrchardCore.Media"
    },
    Description = "Manage meta tags for content items.",
    Category = "Content"
)]