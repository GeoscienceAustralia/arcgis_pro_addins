##MapToolZoom

<!-- TODO: Write a brief abstract explaining this sample -->
ProGuide example for simple MapView interaction is demonstrated in this sample with a tool that allows to zoom in and out of the current MapView.  The left mouse click will zoom in and the right mouse click will zoom out.  Unlike the other Map Tool samples this example does not use the sketch capabilities of the MapTool base class instead the sample overrides mouse and keyboard events.    
  


<a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">View it live</a>

<!-- TODO: Fill this section below with metadata about this sample-->
```
Language:              C# 6.0
Subject:               Framework
Contributor:           ArcGIS Pro SDK Team <arcgisprosdk@esri.com>
Organization:          Esri, http://www.esri.com
Date:                  11/1/2016
ArcGIS Pro:            1.4
Visual Studio:         2015
.NET Target Framework: 4.6.1
```

##Resources

* [API Reference online](http://pro.arcgis.com/en/pro-app/sdk/api-reference)  
* <a href="http://pro.arcgis.com/en/pro-app/sdk/" target="_blank">ArcGIS Pro SDK for .NET (pro.arcgis.com)</a>  
* [arcgis-pro-sdk-community-samples](http://github.com/Esri/arcgis-pro-sdk-community-samples)  
* [ArcGIS Pro DAML ID Reference](http://github.com/Esri/arcgis-pro-sdk/wiki/ArcGIS Pro DAML ID Reference)  
* [FAQ](http://github.com/Esri/arcgis-pro-sdk/wiki/FAQ)  
* [ArcGIS Pro SDK icons](https://github.com/Esri/arcgis-pro-sdk/releases/tag/1.2.0.5023)  
![ArcGIS Pro SDK for .NET Icons](http://esri.github.io/arcgis-pro-sdk/images/Home/Image-of-icons.png "ArcGIS Pro SDK Icons")
* [ProConcepts: ArcGIS Pro Add in Samples](https://github.com/Esri/arcgis-pro-sdk-community-samples/wiki/ProConcepts-ArcGIS-Pro-Add-in-Samples)  

### Samples Data

* Sample data for ArcGIS Pro SDK Community Samples can be downloaded from the [repo releases](https://github.com/Esri/arcgis-pro-sdk-community-samples/releases) page.  

##How to use the sample
<!-- TODO: Explain how this sample can be used. To use images in this section, create the image file in your sample project's screenshots folder. Use relative url to link to this image using this syntax: ![My sample Image](FacePage/SampleImage.png) -->
1. Download the Community Sample data (see under the 'Resources' section for downloading sample data).  The sample data contains a dataset called 'Interacting with Maps' with both 2D and 3D data.  Make sure that the Sample data is unzipped in c:\data and "C:\Data\Interacting with Maps" is available.  
1. Open this solution in Visual Studio 2015.    
1. Click the build menu and select Build Solution.  
1. Click the Start button to open ArCGIS Pro.  ArcGIS Pro will open.  
1. Open the project "Interacting with Maps.aprx" in the "C:\Data\Interacting with Maps" folder since this project contains 2D and 3D data.  
1. Open the 2D crime map  
1. Click on the Add-in tab   
1. Click the 'Zoom In/Out' button and left click on the map somewhere off the map center.  
![UI](Screenshots/3MapTool2D.png)  
  
1. Validate the that the mouse click point is now at the center of the map view and that the view has zoomed in.  
![UI](Screenshots/3MapTool2D-2.png)  
  
1. Switch to the Portland 3D City map view and perform the zoom in/out on the 3D scene.  
![UI](Screenshots/3MapTool3D.png)  
  
1. Validate the zoom in/out functionality.  
![UI](Screenshots/3MapTool3D-2.png)  
  
1. Use the cursor up and cursor down keys and validate that the zoom in/out is working.  
  


[](Esri Tags: ArcGIS-Pro-SDK)
[](Esri Language: C-Sharp)​

<p align = center><img src="http://esri.github.io/arcgis-pro-sdk/images/ArcGISPro.png"  alt="pre-req" align = "top" height = "20" width = "20" >
<b> ArcGIS Pro 1.4 SDK for Microsoft .NET Framework</b>
</p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Home](https://github.com/Esri/arcgis-pro-sdk/wiki) | <a href="http://pro.arcgis.com/en/pro-app/sdk" target="_blank">ArcGIS Pro SDK</a> | <a href="http://pro.arcgis.com/en/pro-app/sdk/api-reference/index.html" target="_blank">API Reference</a> | [Requirements](#requirements) | [Download](#installing-arcgis-pro-sdk-for-net) |  <a href="http://github.com/esri/arcgis-pro-sdk-community-samples" target="_blank">Samples</a>
