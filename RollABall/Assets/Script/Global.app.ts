/* Global Application Script Template */
BABYLON.SceneManager.OnWindowLoad(() => {
    // Global Page Loaded Handler
    console.log("Ready page");
});

BABYLON.SceneManager.OnDeviceReady(() => {
    // Global Device Ready Handler
    console.log("Ready device handler");
});

BABYLON.SceneManager.ExecuteWhenReady((scene: BABYLON.Scene, manager: BABYLON.SceneManager) => {
    // Global Scene Ready Handler
    console.log("Ready scene: ", scene);
});