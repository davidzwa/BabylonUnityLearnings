module PROJECT {
    export class GUIComponent extends BABYLON.MeshComponent {
        GUI: BABYLON.GUI.AdvancedDynamicTexture;
        controls: BABYLON.GUI.Control[] = [];
        titleTxt: BABYLON.GUI.Control;

        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        };

        protected setupGUI(): void {
            this.GUI = BABYLON.GUI.AdvancedDynamicTexture.CreateFullscreenUI("myUI");
        }

        protected addTitle(title: DefaultTextControl) {
            let titleBlock = this.defaultTextBlock(title);
            this.pushControl(titleBlock);
        }

        protected addCounter(counter: DefaultTextControl, initialValue: number = 0) {
            let counterBlock = this.defaultTextBlock(counter);
            counterBlock.text = counter.text + ": " + initialValue.toString();

            this.pushControl(counterBlock);
        }

        protected addMenuButtonWithObservable(buttonProperties: DefaultButtonControl): BABYLON.GUI.Button {
            let menuButton = this.defaultButton(buttonProperties);
            this.pushControl(menuButton);
            return menuButton;
        }

        protected addMenuOverlay() {
            let overlayRect = new BABYLON.GUI.Rectangle("MenuOverlay");
            overlayRect.width = 1;
            overlayRect.height = 1;
            overlayRect.cornerRadius = 0;
            overlayRect.color="black";
            overlayRect.thickness = 4;
            overlayRect.alpha = 0.5;
            overlayRect.background = "black";

            this.pushControl(overlayRect);
        }

w

        protected getControlByClass(klass: string) {
            return this.controls.find(c => c.name === klass);
        }

        protected translate(vert: VertPos, hor: HorPos, offset: Placement = new Placement(0, 0)): Placement {
            let top = -this.GUI.getSize().height / 2;
            let left = -this.GUI.getSize().width / 2;
            if (vert == VertPos.TOP)
                offset.top = top + offset.top;
            else if (vert == VertPos.BOTTOM)
                offset.top = -top + offset.top;

            if (hor == HorPos.LEFT)
                offset.left = left + offset.left;
            else if (hor == HorPos.RIGHT)
                offset.left = -left + offset.left;

            return offset;
        }

        private pushControl(control: BABYLON.GUI.Control) {
            this.controls.push(control);
            this.GUI.addControl(control);
        }

        private defaultTextBlock(control: DefaultTextControl) {
            let defaultTextBlock = new BABYLON.GUI.TextBlock(control.name)
            defaultTextBlock.text = control.text;
            defaultTextBlock.fontSize = 24;
            defaultTextBlock.top = control.position.top;
            defaultTextBlock.left = control.position.left;
            defaultTextBlock.color = control.color ? control.color : "white";
            return defaultTextBlock;
        }

        private defaultButton(control: DefaultButtonControl) {
            let defaultBtn = BABYLON.GUI.Button.CreateSimpleButton(control.name, control.text)
            defaultBtn.fontSize = 24;
            defaultBtn.top = control.position.top;
            defaultBtn.left = control.position.left;
            defaultBtn.height = control.height;
            defaultBtn.width = control.width;
            defaultBtn.background = "#67BC45";
            defaultBtn.color = control.color ? control.color : "white";
            return defaultBtn;
        }

    }

    export interface DefaultButtonControl extends DefaultTextControl {
        width: number,
        height: any
    }

    export interface DefaultTextControl {
        name: string,
        text: string,
        control?: BABYLON.GUI.Control,
        color?: string,
        position?: Placement
    }

    export class Placement {
        top: number;
        left: number;
        constructor(left = 0, top = 0) {
            this.top = top;
            this.left = left;
        }
    }

    export enum VertPos {
        TOP,
        MIDDLE,
        BOTTOM
    }

    export enum HorPos {
        LEFT,
        MIDDLE,
        RIGHT
    }
}
