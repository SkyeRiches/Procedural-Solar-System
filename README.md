# Procedural-Solar-System
This is a project I'm currently working on where the aim is to create a system in the Unity engine where you can, at the click of a button, create a random solar system made up of a star and planets, where the planets all have procedural terrain around their surface. 



The data of each planet will be determined by choosing a random planet from the NASA exo-planet database (https://exoplanetarchive.ipac.caltech.edu/cgi-bin/TblView/nph-tblView?app=ExoTbls&config=PS) and the appearance will be determined by the temperature of the planet and how Earth-like the planet is.



A user running the program should be able to click on a planet and focus in on it with the camera following the planet until the user selects another planet or presses a key to stop following the planet. 



A possible extended aim of the project is to allow the user to manually change the settings of a planet to tweak it more to their liking and the system will automatically readjust the orbit of the planet appropriately.


# Design-Brief
- Create a system for allowing designers to create a custom planet with procedural terrain​
- Create a shader that adds color to the planet based on terrain height and ocean depth
- Create the ability for a designer to add biomes to a planet which will have different color tints to indicate different temperatures
- Implement orbital physics using Newtons law of universal gravity and give planet a spin rate that will be determined by the solar day of the planet
- Create a free-cam to allow a user to fly around the system observing the planets
- Develop the ability for the solar system to be made up of random planets based on the NASA database of exo-planets
- Merge the planet terrain functionality created at the start, with the solar system functionality in order to have a random solar system where the planets have terrain
- Modify the terrain system to take in data from the NASA database for exo-planet temperature and similarity to Earth
- Implement a system to allow a user to click on a planet during run time and modify key values, such as; mass, radius, temperature and terrain roughness. The camera should also focus on that planet and follow it in its orbit whilst it is selected
