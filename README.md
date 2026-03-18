# HamzaYasinAssessmentProject

This should run just by opening it up in VS2026 and hitting run. I've got this set up to run both the Web API and the Maui project at the same time, this may be undone when opening up in a new VS setup. If you get connection errors popping up or only the web service running, then you'll need to set it to run both the MAUI project and Web API projust at the same time. 

There is a slight deviation in the construction from what was requested here. Specifically, the spec asks for both a .Net MAUI project with a fully implemented MVVM setup. It also asks for testing on the MAUI project, using NUnit. I'ce taken some of the functionality out of the front end MAUI solution and put it into a separate "Shared" solution. 

I tested this using a windows emulator. I have not tested on Android or IOS emulators, but it should work although I can't guarantee that it will. This is mostly due to a lack of RAM on the machine I wrote this on.


And this brings us nicely to the request for a request for SOLID architecture. 

Briefly, SOLID principles describes a way for a programmer to intentionally keep each section of code they create dedicated to the specific task it exists for and not responsible for multiple things all in the same place. The reason for this is that it makes code easier to understand and easier to scale up. 

An example of the benefits of SOLID Architecture is demonstrated well by this very project. I encountered an issue when creating the Unit tests for the logic which was already in the MAUI Project. If I had created the logic layer embedded into the View layer, then modifying the solution to get the Unit testing working would have been a tedious task which would have taken a long time and potentially resulting in a full rewrite. 

However due to me creating the application with all parts of the code being responsible for a single thing as much as possible, then it was simple for me to modify it and put all the logic into the shared layer of the solution. it was a case of dragging files to their new location and updating some namespaces to get eveything working as I needed it to. 
