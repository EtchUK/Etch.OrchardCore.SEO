using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "SEO",
    Author = "Moov2",
    Website = "https://moov2.com",
    Version = "0.0.1"
)]

[assembly: Feature(
    Id = "Moov2.OrchardCore.SEO.Redirects",
    Name = "Redirects",
    Description = "Create 301 redirects.",
    Dependencies = new [] 
    {
        "OrchardCore.Title"
    },
    Category = "Content"
)]