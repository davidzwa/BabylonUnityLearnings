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


// GUI = BABYLON.GUI.AdvancedDynamicTexture.CreateFullscreenUI("myUI");
// this.textBlock = new BABYLON.GUI.TextBlock();
// this.textBlock.text = this.title;
// this.textBlock.fontSize = 24;
// this.textBlock.top = -100;
// this.textBlock.color = "white";
// GUI.addControl(this.textBlock);