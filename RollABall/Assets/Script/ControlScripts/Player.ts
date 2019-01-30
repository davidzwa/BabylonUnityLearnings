/* Babylon Mesh Component Template */

module PROJECT {
    export class Player extends BABYLON.MeshComponent {

        count: number = 0;
        speed: number = 0;
        private items: BABYLON.Mesh[] = [];

        public constructor(owner: BABYLON.AbstractMesh,
            scene: BABYLON.Scene,
            tick: boolean = true,
            propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
            this.speed = this.getProperty("speed", 1.0);
        }

        protected ready(): void {
            // Scene execute when ready
            console.log("Player controller started", this.manager);
        }

        protected start(): void {
            // Start component function
            console.log(this.mesh);
        }

        protected update(): void {
            // Update render loop function
            this.updatePlayerMovement(); // Cause input to move ball
        }

        private updatePlayerMovement(): void {
            var vert:number = this.manager.getUserInput(BABYLON.UserInputAxis.Vertical, BABYLON.PlayerNumber.One);
            var hor:number = this.manager.getUserInput(BABYLON.UserInputAxis.Horizontal, BABYLON.PlayerNumber.One);
            
            this.mesh.physicsImpostor.applyImpulse(
                new BABYLON.Vector3(hor * this.speed, 0.0, vert * this.speed), this.mesh.getAbsolutePosition());
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