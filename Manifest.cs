using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "SEO",
    Author = "Moov2",
    Website = "https://moov2.com",
    Version = "0.3.2"
)]

[assembly: Feature(
    Id = "Moov2.OrchardCore.SEO.Redirects",
    Name = "Redirects",
    Description = "Create 301 redirects.",
    Dependencies = new[]
    {
        "OrchardCore.Title"
    },
    Category = "Content"
)]

[assembly: Feature(
    Id = "Moov2.OrchardCore.SEO.RobotsTxt",
    Name = "Robots.txt",
    Description = "Manage contents of robots.txt.",
    Category = "Content"
)]


[assembly: Feature(
    Id = "Moov2.OrchardCore.SEO.HostnameRedirects",
    Name = "Hostname Redirects",
    Description = "Manage hostname redirects.",
    Category = "Content"
)]