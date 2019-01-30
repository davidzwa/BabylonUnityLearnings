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
            this.speed = this.getProperty("speed", 1.0);
        }
        ready() {
            // Scene execute when ready
            console.log("Player controller started", this.manager);
        }
        start() {
            // Start component function
            console.log(this.mesh);
        }
        update() {
            // Update render loop function
            this.updatePlayerMovement(); // Cause input to move ball
        }
        updatePlayerMovement() {
            var vert = this.manager.getUserInput(BABYLON.UserInputAxis.Vertical, BABYLON.PlayerNumber.One);
            var hor = this.manager.getUserInput(BABYLON.UserInputAxis.Horizontal, BABYLON.PlayerNumber.One);
            this.mesh.physicsImpostor.applyImpulse(new BABYLON.Vector3(hor * this.speed, 0.0, vert * this.speed), this.mesh.getAbsolutePosition());
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
