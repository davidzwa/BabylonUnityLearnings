/* Babylon Camera Component Template */
module PROJECT {
    export class BallCamera extends BABYLON.CameraComponent {
        public constructor(owner: BABYLON.Camera, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        }

        private mesh: BABYLON.AbstractMesh;
        private offset: BABYLON.Vector3;

        protected start() :void {
            // Start component function
            var player: any = this.getProperty("player", null);
            if (player) {
                this.mesh = this.scene.getMeshByID(player.id);
                this.offset = this.camera.position.subtract(this.mesh.position);
            }
        }

        protected update() :void {
            if (this.mesh && this.offset) 
                this.camera.position = this.mesh.position.add(this.offset);
            // Update render loop function
        }

        protected after() :void {
            // After render loop function
        }

        protected destroy() :void {
            // Destroy component function
            this.mesh = null;
            this.offset = null;
        }
    }
}