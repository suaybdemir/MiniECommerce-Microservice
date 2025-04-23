import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  Renderer2,
  setClassMetadata,
  ɵɵdefineDirective,
  ɵɵdirectiveInject,
  ɵɵlistener
} from "./chunk-OAR43WPE.js";

// node_modules/flexi-tooltip/fesm2022/flexi-tooltip.mjs
var FlexiTooltipDirective = class _FlexiTooltipDirective {
  constructor(el, renderer) {
    this.el = el;
    this.renderer = renderer;
    this.tooltipPosition = "right";
    this.tooltipElement = null;
  }
  ngOnInit() {
    this.tooltipElement = this.renderer.createElement("div");
    this.renderer.appendChild(this.el.nativeElement, this.tooltipElement);
    this.renderer.setStyle(this.tooltipElement, "position", "absolute");
    this.renderer.setStyle(this.tooltipElement, "background-color", "#333");
    this.renderer.setStyle(this.tooltipElement, "color", "#fff");
    this.renderer.setStyle(this.tooltipElement, "padding", "4px 10px");
    this.renderer.setStyle(this.tooltipElement, "border-radius", "4px");
    this.renderer.setStyle(this.tooltipElement, "font-size", "14px");
    this.renderer.setStyle(this.tooltipElement, "z-index", "10000");
    this.renderer.setStyle(this.tooltipElement, "display", "none");
    this.renderer.setStyle(this.tooltipElement, "white-space", "nowrap");
    const tooltipContent = this.renderer.createElement("span");
    this.renderer.appendChild(this.tooltipElement, tooltipContent);
    const arrow = this.renderer.createElement("div");
    this.renderer.appendChild(this.tooltipElement, arrow);
    this.el.nativeElement.style.position = "relative";
    this.renderer.setStyle(arrow, "position", "absolute");
    this.tooltipElement.style.display = "none";
    this.renderer.setStyle(arrow, "width", "0");
    this.renderer.setStyle(arrow, "height", "0");
    this.renderer.appendChild(this.el.nativeElement, this.tooltipElement);
  }
  onMouseEnter() {
    const title = this.el.nativeElement.getAttribute("title");
    if (title) {
      const tooltipContent = this.tooltipElement.querySelector("span");
      if (tooltipContent) {
        tooltipContent.textContent = title;
      }
      this.renderer.setStyle(this.tooltipElement, "display", "block");
      this.el.nativeElement.removeAttribute("title");
      this.positionTooltip();
      this.tooltipElement.style.display = "block";
    }
  }
  onMouseLeave() {
    if (this.tooltipElement) {
      const tooltipContent = this.tooltipElement.querySelector("span");
      if (tooltipContent) {
        this.el.nativeElement.setAttribute("title", tooltipContent.textContent || "");
      }
      this.tooltipElement.style.display = "none";
    }
  }
  positionTooltip() {
    const elHeight = this.el.nativeElement.getBoundingClientRect().height;
    const elWidth = this.el.nativeElement.getBoundingClientRect().width;
    const tooltipHeight = this.tooltipElement.getBoundingClientRect().height;
    const tooltipWidth = this.tooltipElement.getBoundingClientRect().width;
    if (this.tooltipPosition == "") {
      this.tooltipPosition = "left";
    }
    let topOrBottomPX = "0px";
    let leftOrRightPX = "0px";
    if (this.tooltipPosition == "top") {
      topOrBottomPX = (tooltipHeight + 9) * -1 + "px";
      leftOrRightPX = tooltipWidth > elWidth ? (tooltipWidth - elWidth) / 2 * -1 + "px" : "0px";
    } else if (this.tooltipPosition == "bottom") {
      topOrBottomPX = (tooltipHeight + 9) * -1 + "px";
      leftOrRightPX = tooltipWidth > elWidth ? (tooltipWidth - elWidth) / 2 * -1 + "px" : "0px";
    } else if (this.tooltipPosition == "left") {
      topOrBottomPX = (tooltipHeight > elHeight ? (tooltipHeight - elHeight) / 2 * -1 : (elHeight - tooltipHeight) / 2) + "px";
      leftOrRightPX = (tooltipWidth + 9) * -1 + "px";
    } else if (this.tooltipPosition == "right") {
      topOrBottomPX = (tooltipHeight > elHeight ? (tooltipHeight - elHeight) / 2 * -1 : (elHeight - tooltipHeight) / 2) + "px";
      leftOrRightPX = (tooltipWidth + 9) * -1 + "px";
    }
    const positionLeftOrRight = this.tooltipPosition === "left" || this.tooltipPosition === "right" ? this.tooltipPosition : "left";
    const positionTopOrBottom = this.tooltipPosition === "top" || this.tooltipPosition === "bottom" ? this.tooltipPosition : "top";
    this.renderer.setStyle(this.tooltipElement, positionTopOrBottom, topOrBottomPX);
    this.renderer.setStyle(this.tooltipElement, positionLeftOrRight, leftOrRightPX);
    this.updateArrowStyle(this.tooltipPosition);
  }
  updateArrowStyle(direction) {
    const arrow = this.tooltipElement.querySelector("div");
    if (arrow) {
      this.renderer.removeStyle(arrow, "border-left");
      this.renderer.removeStyle(arrow, "border-right");
      this.renderer.removeStyle(arrow, "border-top");
      this.renderer.removeStyle(arrow, "border-bottom");
      this.renderer.removeStyle(arrow, "top");
      this.renderer.removeStyle(arrow, "bottom");
      this.renderer.removeStyle(arrow, "left");
      this.renderer.removeStyle(arrow, "right");
      switch (direction) {
        case "top":
          this.renderer.setStyle(arrow, "bottom", "-5px");
          this.renderer.setStyle(arrow, "left", "48%");
          this.renderer.setStyle(arrow, "border-left", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-right", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-top", "5px solid #333");
          break;
        case "bottom":
          this.renderer.setStyle(arrow, "top", "-5px");
          this.renderer.setStyle(arrow, "left", "48%");
          this.renderer.setStyle(arrow, "border-left", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-right", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-bottom", "5px solid #333");
          break;
        case "right":
          this.renderer.setStyle(arrow, "top", "34%");
          this.renderer.setStyle(arrow, "left", "-5px");
          this.renderer.setStyle(arrow, "border-top", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-bottom", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-right", "5px solid #333");
          break;
        case "left":
        case "":
        default:
          this.renderer.setStyle(arrow, "top", "34%");
          this.renderer.setStyle(arrow, "right", "-5px");
          this.renderer.setStyle(arrow, "border-top", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-bottom", "5px solid transparent");
          this.renderer.setStyle(arrow, "border-left", "5px solid #333");
          break;
      }
    }
  }
  static {
    this.ɵfac = function FlexiTooltipDirective_Factory(__ngFactoryType__) {
      return new (__ngFactoryType__ || _FlexiTooltipDirective)(ɵɵdirectiveInject(ElementRef), ɵɵdirectiveInject(Renderer2));
    };
  }
  static {
    this.ɵdir = ɵɵdefineDirective({
      type: _FlexiTooltipDirective,
      selectors: [["", "flexiTooltip", ""]],
      hostBindings: function FlexiTooltipDirective_HostBindings(rf, ctx) {
        if (rf & 1) {
          ɵɵlistener("mouseenter", function FlexiTooltipDirective_mouseenter_HostBindingHandler() {
            return ctx.onMouseEnter();
          })("mouseleave", function FlexiTooltipDirective_mouseleave_HostBindingHandler() {
            return ctx.onMouseLeave();
          });
        }
      },
      inputs: {
        tooltipPosition: [0, "flexiTooltip", "tooltipPosition"]
      }
    });
  }
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(FlexiTooltipDirective, [{
    type: Directive,
    args: [{
      selector: "[flexiTooltip]",
      standalone: true
    }]
  }], () => [{
    type: ElementRef
  }, {
    type: Renderer2
  }], {
    tooltipPosition: [{
      type: Input,
      args: ["flexiTooltip"]
    }],
    onMouseEnter: [{
      type: HostListener,
      args: ["mouseenter"]
    }],
    onMouseLeave: [{
      type: HostListener,
      args: ["mouseleave"]
    }]
  });
})();

export {
  FlexiTooltipDirective
};
//# sourceMappingURL=chunk-6DJ7HQQN.js.map
