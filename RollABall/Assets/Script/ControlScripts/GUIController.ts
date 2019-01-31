/* Babylon Scene Controller Template */

var GUI;
module PROJECT {
    export class GUIController extends BABYLON.MeshComponent {
        title: string = "";
        textBlock: BABYLON.GUI.TextBlock;
        GUI: BABYLON.GUI.AdvancedDynamicTexture;
        playerCount: number = 0;

        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
            this.title = this.getProperty("Title",'asd');
        }

        protected ready(): void {
            // Scene execute when ready
        }

        protected start(): void {
            // Start component function
        }

        protected update(): void {
            // Update render loop function
            // this.textBlock.text = this.title;
            // console.log(this.GUI.getSize().height / 2);
        }

        protected after(): void {
            // After render loop function
        }

        protected destroy(): void {
            // Destroy component function
        }
    }
}