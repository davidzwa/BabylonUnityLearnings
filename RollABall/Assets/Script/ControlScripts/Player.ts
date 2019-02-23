/* Babylon Mesh Component Template */
/// <reference path="../Models/GUIComponent.ts" />

module PROJECT {
    export class Player extends BABYLON.MeshComponent {
        GUI: GUIController;

        count: number = 0;
        speed: number = 0;
        items: BABYLON.Mesh[] = [];

        public constructor(owner: BABYLON.AbstractMesh,
            scene: BABYLON.Scene,
            tick: boolean = true,
            propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        }

        protected ready(): void {
            // Scene execute when ready
            console.log("Player controller started", this.manager);

            try {
                console.log("GUIController1", this.scene.getMeshByName("GUIController101"));
            } catch (e) {
                console.log("Exception:", e);
                debugger;
            }

            this.GUI = this.scene.getMeshByName("GUIController101").metadata.components[0].instance;
        }

        protected start(): void {
            // Start component function
            this.speed = this.getProperty("speed", 1.0);
            this.items = this.scene.getMeshesByTags("Pickup");
            this.count = 0;
            this.updateCollectionCount();

            this.GUI.updateTitle('Text2');
        }

        protected update(): void {
            // Update render loop function
            this.updatePlayerMovement(); // Cause input to move ball

            this.updatePickupCollisions();
            this.updateCollectionCount();
        }

        private updatePlayerMovement(): void {
            var vert: number = this.manager.getUserInput(BABYLON.UserInputAxis.Vertical, BABYLON.PlayerNumber.One);
            var hor: number = this.manager.getUserInput(BABYLON.UserInputAxis.Horizontal, BABYLON.PlayerNumber.One);

            this.mesh.physicsImpostor.applyImpulse(
                new BABYLON.Vector3(hor * this.speed, 0.0, vert * this.speed), this.mesh.getAbsolutePosition());
        }

        private updatePickupCollisions(): void {
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

        private updateCollectionCount() {
            this.GUI.updateScore(this.count.toString());

        }

        protected after(): void {
            // After render loop function
        }

        protected destroy(): void {
            // Destroy component function
            console.log("Player controller destroyed");
        }
    }
}