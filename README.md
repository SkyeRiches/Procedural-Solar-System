# Procedural Solar System
This was a project developed for an assessment for a university module. We were tasked with creating some sort of procedural system. For my project I decided I would make a system that allows a user to create a solar system of planets each with procedural terrain.

# Functionality
When the user presses the button to create a planet, it creates an instance of the planet prefab and gives each of the terrain settings and color settings a random value between certain specific values that create a planet terrain that looks plausible and realistic.

Each planet created rotates around the central star and a line renderer plots the route that the planet is following so that the user can see the orbital path.

The planets created are also given a color tint based on how close it is to the central star, blue tint when it is further away and red tint when it is closer. This is used to visualise the temperature of the planet. 

A user running the program is able to click on a planet and focus in on it with the camera following the planet until the user selects another planet or presses a key to stop following the planet. Whilst focusing in on a planet the user is able to tweak the settings of the planet, including the terrain and coloration of the planets surface and color tint.
