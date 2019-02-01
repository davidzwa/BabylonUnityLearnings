module PROJECT {
    export class GUIComponent extends BABYLON.SceneComponent {
        GUI: BABYLON.GUI.AdvancedDynamicTexture;
        controls: BABYLON.GUI.Control[] = [];
        titleTxt: BABYLON.GUI.Control;

        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        };

        protected setupGUI(): void {
            this.GUI = BABYLON.GUI.AdvancedDynamicTexture.CreateFullscreenUI("myUI");
        }

        protected addDefaultTitle(title: DefaultTextControl) {
            let titleBlock = this.defaultTextBlock(title);
            this.GUI.addControl(titleBlock);
            this.controls.push(titleBlock);
        }

        protected addBottomCounter(counter: DefaultTextControl, initialValue: number = 0) {
            let titleBlock = this.defaultTextBlock(counter);
            titleBlock.text = counter.text + ": " + initialValue.toString();

            this.GUI.addControl(titleBlock);
            this.controls.push(titleBlock);
        }

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

        private defaultTextBlock(control: DefaultTextControl) {
            let defaultTextBlock = new BABYLON.GUI.TextBlock(control.name)
            defaultTextBlock.text = control.text;
            defaultTextBlock.fontSize = 24;
            defaultTextBlock.top = control.position.top;
            defaultTextBlock.left = control.position.left;
            defaultTextBlock.color = control.color ? control.color : "white";
            return defaultTextBlock;
        }

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
        constructor(top = 0, left = 0) {
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
