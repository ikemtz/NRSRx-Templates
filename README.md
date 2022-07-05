[![Build Status](https://ikemtz.visualstudio.com/Devops/_apis/build/status/NRSRx?branchName=master)](https://ikemtz.visualstudio.com/Devops/_build/latest?definitionId=32&branchName=master)
[![Release Status](https://ikemtz.vsrm.visualstudio.com/_apis/public/Release/badge/9abb8a0b-71e1-4090-b59c-46edc077875f/20/20)](https://ikemtz.visualstudio.com/Devops/_release?_a=releases&view=mine&definitionId=20)
[![Nuget Package](https://img.shields.io/nuget/v/IkeMtz.NRSRx.Templates.svg)](https://www.nuget.org/packages?q=nrsrx) 
[![Nuget Downloads](https://img.shields.io/nuget/dt/IkeMtz.NRSRx.Templates)](https://www.nuget.org/packages/IkeMtz.NRSRx.Templates/)

# NRSRx Templates
These are templates for starting new [NRSRx](https://github.com/ikemtz/NRSRx) projects.  These templates are fully compatible with Visual Studio 2022 and can also be used via the [dotnet-cli](https://docs.microsoft.com/en-us/dotnet/core/tools/).

## Requirements

* The latest dotnet SDK.
* Visual Studio 2022 (if you're planning to use Visual Studio)

## Getting started
At your closest command prompt, run the following:

```dotnet new -i IkeMtz.NRSRx.Templates```

The above command is also useful for pulling the latest version of the templates.  It's **ALWAYS** a good idea to generate your projects off of the latest template.

## Scripting Model, OData, WebApi Services, and test projects

You can either go through the steps within Visual Studio or use the following script.  

**Note**: One drawback to this approach is that you'll have to add the generated projects to your solution file manually.

## Naming Standards

NRSRx is an opinionated framework, which is also true for its templates.  All domain/entity names should be in singular form.

** Do **
```Sample.Student.Models```

** Don't **
```Sample.Students.Models```

The following naming standards should also be observed:
* Model projects should end with a ".Models" suffix.
* OData projects should end with a ".OData" suffix.
* WebApi projects should end with a ".WebApi" suffix.
* Test projects should end with a ".Tests" suffix.

## Scripting

### Scripting a models project:
```dotnet new nrsrx-models -n {$Your Domain Name}.Models```

### Scripting an OData project
```dotnet new nrsrx-odata -n {$Your Domain Name}.OData```

### Scripting an OData Test project
```dotnet new nrsrx-odata-test -n {$Your Domain Name}.OData.Tests```

### Scripting a WebApi project
```dotnet new nrsrx-webapi -n {$Your Domain Name}.WebApi```

### Scripting a WebApi Test project
```dotnet new nrsrx-webapi-test -n {$Your Domain Name}.WebApi.Tests```
