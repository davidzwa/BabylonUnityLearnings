/* Babylon Scene Controller Template */

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

            this.GUI = BABYLON.GUI.AdvancedDynamicTexture.CreateFullscreenUI("myUI");
            this.textBlock = new BABYLON.GUI.TextBlock();
            this.textBlock.text = this.title;
            this.textBlock.fontSize = 24;
            this.textBlock.top = -100;
            this.textBlock.color = "white";
            this.GUI.addControl(this.textBlock);
        }

        protected start(): void {
            // Start component function
        }

        protected update(): void {
            // Update render loop function
            // this.textBlock.text = this.title;
        }

        protected after(): void {
            // After render loop function
        }

        protected destroy(): void {
            // Destroy component function
        }
    }
}