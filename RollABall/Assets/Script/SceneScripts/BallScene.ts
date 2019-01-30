/* Babylon Mesh Component Template */

module PROJECT {
    export class BallScene extends BABYLON.MeshComponent {
        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        }

        protected ready() :void {
            
            console.log("Ready BallScene");
            // Scene execute when ready
        }

        protected start() :void {
            console.log("Started BallScene");
            // Start component function
        }

        protected update() :void {
            // Update render loop function
        }

        protected after() :void {
            // After render loop function
        }

        protected destroy() :void {
            console.log("Destroyed BallScene");
            // Destroy component function
        }
    }
}