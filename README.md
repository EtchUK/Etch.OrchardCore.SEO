# Etch.OrchardCore.SEO

Module for [Orchard Core](https://github.com/OrchardCMS/OrchardCore) that provides useful features for SEO (search engine optimisation).

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.SEO.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.SEO) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.SEO.svg)](https://www.nuget.org/packages/Etch.OrchardCore.SEO)

## Orchard Core Reference

This module is referencing a stable build of Orchard Core ([`1.1.0`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.1.0)).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.SEO). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.SEO", ensuring include prereleases is checked.

## Features

### Hostname redirects

Define main hostname to redirect all domain variations and force SSL.

### Meta Tags

Attach content part that gives content editors ability to manage basic meta tags (inspired by [metatags.io](https://metatags.io)).

### Redirects

Create redirect content items that'll redirect a relative URL to another URL.

#### Known Issues

When creating a content item to redirect from homepage to another page, you need to make sure to disable the `Home Route` Module to make the redirect work correctly.

#### Import

This feature adds the ability to bulk import redirects from an XLSX file.

### Robots.txt

Manage contents of `/robots.txt` or block search engines with [noindex](https://developers.google.com/search/docs/advanced/crawling/block-indexing).
