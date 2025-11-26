# UK Legislation API - Endpoint Documentation

## Overview

The legislation.gov.uk API does **NOT** provide native JSON responses. Instead, it uses XML-based formats.

## Supported Formats

### 1. XML (CLML - Crown Legislation Markup Language)
- **URL Pattern**: `/{type}/{year}/{number}/data.xml`
- **Example**: `https://www.legislation.gov.uk/ukpga/1998/42/data.xml`
- **Use Case**: Full legislation content in structured XML format
- **Status**: ✅ Working

### 2. Atom Feeds (XML)
- **URL Pattern**: `/{type}/{year}/data.feed` or `/{type}/data.feed`
- **Example**: `https://www.legislation.gov.uk/ukpga/1998/data.feed`
- **Use Case**: Lists of legislation, search results
- **Status**: ✅ Working

### 3. RDF (Linked Data)
- **URL Pattern**: `/{type}/{year}/{number}/data.rdf`
- **Example**: `https://www.legislation.gov.uk/ukpga/1998/42/data.rdf`
- **Use Case**: Semantic web, linked data applications
- **Status**: ✅ Working (not currently implemented)

### 4. HTML
- **URL Pattern**: `/{type}/{year}/{number}`
- **Example**: `https://www.legislation.gov.uk/ukpga/1998/42`
- **Use Case**: Human-readable web interface
- **Status**: ✅ Working

### 5. JSON
- **URL Pattern**: N/A
- **Status**: ❌ Not supported by the API

## Implementation Strategy

Since JSON is not natively supported, we have two options:

### Option A: XML Parsing (Recommended)
Parse the XML responses directly:
- Use `System.Xml` or `System.Xml.Linq` for CLML parsing
- Use Atom feed parsers for list/search responses
- Serialize to our C# models

**Pros**:
- Official API format
- Complete data available
- Most reliable

**Cons**:
- More complex parsing logic
- CLML schema is extensive

### Option B: Screen Scraping HTML
Parse the HTML responses:
- Extract data from HTML structure
- Convert to our models

**Pros**:
- Simpler than XML for some cases

**Cons**:
- Fragile (HTML structure may change)
- Not an official API approach
- May miss metadata

### Option C: Hybrid Approach (Chosen)
- Use Atom feeds for lists and search (easier to parse than full CLML)
- Use XML (CLML) for detailed legislation content when needed
- Provide extension points for RDF if needed later

## Endpoint Patterns

### Legislation Content
```
GET /{type}/{year}/{number}/data.xml
Returns: Full legislation in CLML XML format

GET /{type}/{year}/{number}/section/{number}/data.xml
Returns: Specific section in XML format

GET /{type}/{year}/{number}/{date}/data.xml
Returns: Point-in-time version in XML format

GET /{type}/{year}/{number}/enacted/data.xml
Returns: Original enacted version in XML format
```

### Lists and Search
```
GET /{type}/data.feed
Returns: Atom feed of all legislation of specified type

GET /{type}/{year}/data.feed  
Returns: Atom feed of legislation for specific type and year

GET /search?text={query}&type={type}&year={year}
Returns: HTML search results (may need to request with Accept: application/atom+xml header)
```

### Provision Endpoints (❌ Not Available)

Provision-specific endpoints do not appear to be supported:
```
GET /{type}/{year}/{number}/section/{number}/data.xml
Returns: 404 Not Found
```

**Workaround**: Retrieve the full legislation XML and parse specific sections from it.

### Metadata
```
GET /{type}/{year}/{number}/data.xml?view=snippet
Returns: Metadata and excerpt

GET /{type}/{year}/{number}/contents/data.xml
Returns: Table of contents in XML format ✅ Working
```

## Content Negotiation

The API respects the `Accept` header:
- `application/xml` or `text/xml` → XML response
- `application/atom+xml` → Atom feed (for lists)
- `application/rdf+xml` → RDF response
- `text/html` → HTML response (default)

## Examples

### Get Human Rights Act 1998 (Full XML)
```
GET https://www.legislation.gov.uk/ukpga/1998/42/data.xml
Accept: application/xml
```

### List all UK Public General Acts from 1998
```
GET https://www.legislation.gov.uk/ukpga/1998/data.feed
Accept: application/atom+xml
```

### Get Section 1 of Human Rights Act 1998
```
GET https://www.legislation.gov.uk/ukpga/1998/42/section/1/data.xml
Accept: application/xml
```

### Point-in-Time: Human Rights Act as at 1st Jan 2020
```
GET https://www.legislation.gov.uk/ukpga/1998/42/2020-01-01/data.xml
Accept: application/xml
```

## Response Sizes

- Individual legislation XML files can be large (100KB - 10MB+)
- Atom feeds are smaller (typically <100KB per page)
- Consider streaming for large documents

## Rate Limiting

- No documented rate limits
- Be respectful: implement exponential backoff
- Cache responses where appropriate

## References

- Official Documentation: https://www.legislation.gov.uk/developer/contents
- Formats: https://www.legislation.gov.uk/developer/formats
- URIs: https://www.legislation.gov.uk/developer/uris
- CLML Schema: Available in XML responses
