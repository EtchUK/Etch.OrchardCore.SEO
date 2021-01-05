# Etch.OrchardCore.SEO

Module for [Orchard Core](https://github.com/OrchardCMS/OrchardCore) that provides useful features for SEO (search engine optimisation).

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.SEO.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.SEO) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.SEO.svg)](https://www.nuget.org/packages/Etch.OrchardCore.SEO)

## Orchard Core Reference

This module is referencing the RC2 build of Orchard Core ([`1.0.0-rc2-13450`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.0.0-rc2-13450)).

## Features

### Hostname redirects

Define main hostname to redirect all domain variations and force SSL.

### Meta Tags

Attach content part that gives content editors ability to manage basic meta tags (inspired by [metatags.io](https://metatags.io)).

#### Known Issues

When using this module you may notice the page title will include the display text for the content item as well as the value defined in the title field. This is because the [`ContentsMetadata`](https://github.com/OrchardCMS/OrchardCore/blob/dev/src/OrchardCore.Modules/OrchardCore.Contents/Views/ContentsMetadata.cshtml) shape within Orchard Core will automatically add the display text for a content item to a page title. The work around is to override the `ContentsMetadata` shape within your theme. Below is the Razor template that will set the page title to the value entered in the meta tag title field. When the content type doesn't have the `MetaTagsPart` attached or no value has been defined within the title field the default approach of using the display text for the content item is used.

```
@using OrchardCore.ContentManagement

@{
    ContentItem contentItem = Model.ContentItem;

    if (Model.ContentItem.Content.MetaTagsPart == null || string.IsNullOrWhiteSpace((string)Model.ContentItem.Content.MetaTagsPart.Title)) {
        Title.AddSegment(Html.Raw(Html.Encode(contentItem.DisplayText)));
    }
}
```

_Under development_

### Redirects

Create redirect content items that'll redirect a relative URL to another URL.

#### Known Issues

When creating a content item to redirect from homepage to another page, you need to make sure to disable the `Home Route` Module to make the redirect work correctly.

#### Import

This feature adds the ability to bulk import redirects from an XLSX file.

### Robots.txt

Manage contents of `/robots.txt` or block search engines with [noindex](https://developers.google.com/search/docs/advanced/crawling/block-indexing).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.SEO). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.SEO", ensuring include prereleases is checked.
