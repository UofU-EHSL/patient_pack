# patient_pack

  This Unity project is used to store a number of different medical focused assets. From 3D model to code frameworks.

### Dental

  Thus far we only have the 3D models form our dental system in here. Those files can be found under Dental Assets. They include textures and low poly versions of a lot of the major tools that would be used in a dental practice.

### Hospital

  Under the folder Hospital Assets you will find models with textures and built into prefabs. These all come from our trauma sim and so they are all focused to emergent case equipment and rooms.

### Patient Pack

  Patient pack is a folder in the project that if you want to use in a different project then you will need to export it as a unity asset and then import it into a new project. When you look at the vital monitor scene in this directory you will see a fully functional and dynamic vital monitor. You will also notice in the top menu bar you now have an option for "Patient pack" this is used to create new treatment and assesments. They both work off of the Treatment scrip and if you create one of them it will create a new game object with the options you put into the create wizard.

  Once you have a treatment or assesment you can move it into the 'Patient Monitor -> vital mods' and it will start to modify the vitals. You can place treatments, assessments, or conditions in any order.

### Volumetric rendering and cutting

We have added a few different packages from other amazing github projects out there that are different ways of visualising a mesh.
  - Voxel is used to make a minecraft looking mesh out of cubes that make it each to start cutting into the mesh, but they are relatively block and can make smooth looking cuts hard to obtain.
  - UnityVolumeRendering uses ray marching to build fully volumetric renders from dicom data. The benefits this has are all visual. When used correctly it's a very powerful way to look at dicom data. The down sides are the performance. So in our demo scenes you will see we are using render textures and low res virtal cameras to make it so the ray marching works on the quest (still not well but does run). The goal of this is to use it for doing cuts on dicom data.

### Drive post

  This is used to post data to a google drive doc. You first need to make a form on forms.google.com and each question will be the header in the sheet. You have to have each question set to a form of a string so short answer or long answer. Then you have to inspect the element of the forms when you are viewing it and find the entry ID it will look something like this entry.395933536 we then have to put the BASE_URL in so that will be something like https://docs.google.com/forms/d/e/ABunchOfRandomLookingLetters Make sure that it does NOT have the /viewform or /edit at the end. Your own script should be what changes the values then you can call the submit function. You can see an example of this in the trauma project under the MenuTracking script.
