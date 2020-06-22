# Registration Free Assembly

## Dll Server setup

1. under linker settings set register to false
2. add xml manifest.xml and populate with

	```XML
	<?xml version="1.0" encoding="utf-8"?>
	<assembly xmlns="urn:schemas-microsoft-com:asm.v1" manifestVersion="1.0">
		<assemblyIdentity type="win32" name="{LIBRARY NAME}" version="1.0.0.0" />
		<file name="{ASSEMBLY FILENAME}">
			<comClass clsid="{COCLASS UUID}" progid="{FRIENDLY NAME FOR CLASS}" threadingModel="Both" />
			<!-- repeat above line for each class provided by the library -->
		</file>
	</assembly>
	```

3. edit post build step and add: ```mt -manifest Manifest.xml -outputresource:"$(TargetPath)";2 -nologo```.  This will overwrite the default manifest with the more limited content, by default the manifest will be for executable which isn't appropriate for an assembly

## Client setup

1. add manifest.xml and configure as server with the exception of the post build step (we want the full manifest for exe, if client is another dll then repeat the above step but keep in mind the final executable will also need this, and the following steps - a chain of manifests is needed)

	```XML
	<?xml version="1.0" encoding="utf-8"?>
	<assembly manifestVersion="1.0" xmlns="urn:schemas-microsoft-com:asm.v1">
	  <assemblyIdentity version="1.0.0.0" name="{EXE ASSEMBLY NAME}"/>
	  <dependency>
		<dependentAssembly>
		  <assemblyIdentity type="win32" name="{DEPENDENT lIBRARY NAME}" version="1.0.0.0" />
		</dependentAssembly>
	  </dependency>
	```

## Additional Notes

- regfree COM can only work with dlls, out of process servers need to be registered
- in theory .NET can be exposed this way as well and I've had some success with this at least as far as finding the type but have thus far been unable to get the CLR to properly initialize 
