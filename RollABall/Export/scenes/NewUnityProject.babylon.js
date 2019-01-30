window.project = true;

// Project Shader Store


// Browser Window Services

//////////////////////////////////////////////
// Babylon Toolkit - Browser Window Services
//////////////////////////////////////////////

/** Firelight Audio Shims */
window.firelightAudio = 0;
window.firelightDebug = false;
if (window.firelightAudio === 1 || window.firelightAudio === 2) {
	var fmodjs = "scripts/fmodstudio.js";
	if (window.firelightDebug === true) {
		fmodjs = ("scripts/" + (window.firelightAudio === 1) ? "fmodstudioL.js" : "fmodL.js");
	} else {
		fmodjs = ("scripts/" + (window.firelightAudio === 1) ? "fmodstudio.js" : "fmod.js");
	}
	var script2 = document.createElement('script');
	script2.setAttribute("type","text/javascript");
	script2.setAttribute("src", fmodjs);
	if (document.head != null) {
		document.head.appendChild(script2);
	} else if (document.body != null) {
		document.body.appendChild(script2);
	}
}

/** Windows Launch Mode */
window.preferredLaunchMode = 0;
if (typeof Windows !== "undefined" && typeof Windows.UI !== "undefined" && typeof Windows.UI.ViewManagement !== "undefined" &&typeof Windows.UI.ViewManagement.ApplicationView !== "undefined") {
	Windows.UI.ViewManagement.ApplicationView.preferredLaunchWindowingMode = (window.preferredLaunchMode === 1) ? Windows.UI.ViewManagement.ApplicationViewWindowingMode.fullScreen : Windows.UI.ViewManagement.ApplicationViewWindowingMode.auto;
}

/** Xbox Full Screen Shims */
document.querySelector('style').textContent += "@media (max-height: 1080px) { @-ms-viewport { height: 1080px; } }";

/** Xbox Live Plugin Shims */
window.xboxLiveServices = false;
window.isXboxLivePluginEnabled = function() {
	var isXboxLive = (typeof Windows !== "undefined" && typeof Microsoft !== "undefined" && typeof Microsoft.Xbox !== "undefined" && typeof Microsoft.Xbox.Services !== "undefined");
	var hasToolkit = (typeof BabylonToolkit !== "undefined" && typeof BabylonToolkit.XboxLive !== "undefined" && typeof BabylonToolkit.XboxLive.Plugin !== "undefined");
	return (window.xboxLiveServices === true && isXboxLive === true && hasToolkit === true);
}

/** Generic Promise Shims */
window.createGenericPromise = function(resolveRejectHandler) {
	return new Promise(resolveRejectHandler);
}
window.resolveGenericPromise = function(resolveObject) {
    return Promise.resolve(resolveObject);
}


// NewUnityProject.ts
/* Global Application Script Template */
BABYLON.SceneManager.OnWindowLoad(() => {
    // Global Page Loaded Handler
    console.log("Ready page");
});
BABYLON.SceneManager.OnDeviceReady(() => {
    // Global Device Ready Handler
    console.log("Ready device handler");
});
BABYLON.SceneManager.ExecuteWhenReady((scene, manager) => {
    // Global Scene Ready Handler
    console.log("Ready scene: ", scene);
});
/* Babylon Camera Component Template */
var PROJECT;
/* Babylon Camera Component Template */
(function (PROJECT) {
    class BallCamera extends BABYLON.CameraComponent {
        constructor(owner, scene, tick = true, propertyBag = {}) {
            super(owner, scene, tick, propertyBag);
        }
        start() {
            // Start component function
            var player = this.getProperty("player", null);
            if (player) {
                this.mesh = this.scene.getMeshByID(player.id);
                this.offset = this.camera.position.subtract(this.mesh.position);
            }
        }
        update() {
            if (this.mesh && this.offset)
                this.camera.position = this.mesh.position.add(this.offset);
            // Update render loop function
        }
        after() {
            // After render loop function
        }
        destroy() {
            // Destroy component function
            this.mesh = null;
            this.offset = null;
        }
    }
    PROJECT.BallCamera = BallCamera;
})(PROJECT || (PROJECT = {}));
/* Babylon Scene Controller Template */
var PROJECT;
/* Babylon Scene Controller Template */
(function (PROJECT) {
    class GUIController extends BABYLON.MeshComponent {
        constructor(owner, scene, tick = true, propertyBag = {}) {
            super(owner, scene, tick, propertyBag);
            this.title = "";
            this.playerCount = 0;
            this.title = this.getProperty("Title", 'asd');
        }
        ready() {
            // Scene execute when ready
            this.GUI = BABYLON.GUI.AdvancedDynamicTexture.CreateFullscreenUI("myUI");
            this.textBlock = new BABYLON.GUI.TextBlock();
            this.textBlock.text = this.title;
            this.textBlock.fontSize = 24;
            this.textBlock.top = -100;
            this.textBlock.color = "white";
            this.GUI.addControl(this.textBlock);
        }
        start() {
            // Start component function
        }
        update() {
            // Update render loop function
            // this.textBlock.text = this.title;
        }
        after() {
            // After render loop function
        }
        destroy() {
            // Destroy component function
        }
    }
    PROJECT.GUIController = GUIController;
})(PROJECT || (PROJECT = {}));
/* Babylon Mesh Component Template */
var PROJECT;
/* Babylon Mesh Component Template */
(function (PROJECT) {
    class Player extends BABYLON.MeshComponent {
        constructor(owner, scene, tick = true, propertyBag = {}) {
            super(owner, scene, tick, propertyBag);
            this.count = 0;
            this.speed = 0;
            this.items = [];
        }
        ready() {
            // Scene execute when ready
            console.log("Player controller started", this.manager);
        }
        start() {
            // Start component function
            this.speed = this.getProperty("speed", 1.0);
            this.items = this.scene.getMeshesByTags("Pickup");
            this.count = 0;
            this.updateCollectionCount();
        }
        update() {
            // Update render loop function
            this.updatePlayerMovement(); // Cause input to move ball
            this.updatePickupCollisions();
            this.updateCollectionCount();
        }
        updatePlayerMovement() {
            var vert = this.manager.getUserInput(BABYLON.UserInputAxis.Vertical, BABYLON.PlayerNumber.One);
            var hor = this.manager.getUserInput(BABYLON.UserInputAxis.Horizontal, BABYLON.PlayerNumber.One);
            this.mesh.physicsImpostor.applyImpulse(new BABYLON.Vector3(hor * this.speed, 0.0, vert * this.speed), this.mesh.getAbsolutePosition());
        }
        updatePickupCollisions() {
            if (this.items.length > 0) {
                this.items.forEach((item) => {
                    if (item && item.intersectsMesh(this.mesh)) {
                        if (item.isEnabled()) {
                            item.setEnabled(false);
                            this.count += 1;
                            this.updateCollectionCount();
                        }
                    }
                });
            }
        }
        updateCollectionCount() {
            // this.element.innerHTML = "Count: " + this.count.toString();
            // if (this.count >= 12) {
            //     this.winner.className = "";
            // }
        }
        after() {
            // After render loop function
        }
        destroy() {
            // Destroy component function
            console.log("Player controller destroyed");
        }
    }
    PROJECT.Player = Player;
})(PROJECT || (PROJECT = {}));
/* Babylon Mesh Component Template */
var PROJECT;
/* Babylon Mesh Component Template */
(function (PROJECT) {
    class BallScene extends BABYLON.MeshComponent {
        constructor(owner, scene, tick = true, propertyBag = {}) {
            super(owner, scene, tick, propertyBag);
        }
        ready() {
            console.log("Ready BallScene");
            // Scene execute when ready
        }
        start() {
            console.log("Started BallScene");
            // Start component function
        }
        update() {
            // Update render loop function
        }
        after() {
            // After render loop function
        }
        destroy() {
            console.log("Destroyed BallScene");
            // Destroy component function
        }
    }
    PROJECT.BallScene = BallScene;
})(PROJECT || (PROJECT = {}));
/* Babylon Mesh Component Template */
/// <reference path="../../Babylon/Library/babylon.d.ts" />
/// <reference path="../../Babylon/Library/babylon.manager.d.ts" />
var PROJECT;
/* Babylon Mesh Component Template */
/// <reference path="../../Babylon/Library/babylon.d.ts" />
/// <reference path="../../Babylon/Library/babylon.manager.d.ts" />
(function (PROJECT) {
    class CubeComponent extends BABYLON.MeshComponent {
        constructor(owner, scene, tick = true, propertyBag = {}) {
            super(owner, scene, tick, propertyBag);
            this.owner = owner;
        }
        ready() {
            // Scene execute when ready
        }
        start() {
            // Start component function
            this.owner.position = new BABYLON.Vector3(this.rand(), this.rand(6), this.rand());
        }
        update() {
            // Update render loop function
        }
        after() {
            // After render loop function
        }
        destroy() {
            // Destroy component function
        }
        rand(num = 10) {
            return Math.round(Math.random() * num);
        }
    }
    PROJECT.CubeComponent = CubeComponent;
})(PROJECT || (PROJECT = {}));
/* Babylon Mesh Component Template */
var PROJECT;
/* Babylon Mesh Component Template */
(function (PROJECT) {
    class ItemRotator extends BABYLON.MeshComponent {
        constructor(owner, scene, tick = true, propertyBag = {}) {
            super(owner, scene, tick, propertyBag);
            this.speed = 0;
        }
        ready() {
            // Scene execute when ready
        }
        start() {
            // Start component function
            this.speed = this.getProperty("speed", 0.01);
        }
        update() {
            // Update render loop function
            var delta = this.engine.getDeltaTime();
            this.mesh.rotate(new BABYLON.Vector3(1, 1, 1).multiplyByFloats(delta, delta, delta), this.speed);
        }
        after() {
            // After render loop function
        }
        destroy() {
            // Destroy component function
        }
    }
    PROJECT.ItemRotator = ItemRotator;
})(PROJECT || (PROJECT = {}));


