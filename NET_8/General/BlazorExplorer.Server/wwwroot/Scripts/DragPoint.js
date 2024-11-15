class DragPointHelper {
  static dotNetHelper;
  static startY = 0;
  static offsetY = 0;
  static y = 0;

  // save ref
  static setDotNetHelper(value) {
    DragPointHelper.dotNetHelper = value;
  }

  // called from .NET
  static async updateData(x, y) {
    DragPointHelper.dotNetHelper.invokeMethodAsync('UpdateData', x, y);
  }

  static async hitTest(x, y) {
    return DragPointHelper.dotNetHelper.invokeMethodAsync('HitTest', x, y)
  }

  static selectPoint(x, y) {
    let host = document.getElementById("host")
    let handle = document.getElementById("handle")
    const rect = host.getBoundingClientRect()
    handle.style.left = (x + rect.left).toString() + 'px'
    handle.style.top = (y + rect.top).toString() + 'px'
  }

  // event handlers
  static hostPointerDown(e) {
    DragPointHelper.hitTest(e.offsetX, e.offsetY).then(data => {
      let handle = document.getElementById("handle")
      if (data) {
        const rect = this.getHostRect()
        handle.style.left = (data.x + rect.left).toString() + 'px'
        handle.style.top = (data.y + rect.top).toString() + 'px'
        handle.style.visibility = 'visible'
      } else {
        handle.style.visibility = 'hidden'
      }
    })
  }

  static chartDown(e) {
    
  }

  static pointerDown(e) {
    // start moving
    e.stopPropagation()
    this.startY = e.clientY;
    this.y = parseFloat(e.srcElement.style.top)
    e.srcElement.addEventListener('pointermove', this.pointerMove)
    e.srcElement.setPointerCapture(e.pointerId)
  }

  static pointerMove = (e) => {
      // update position
      this.offsetY = this.y + e.clientY - this.startY
      const rect = this.getHostRect()
      this.offsetY = this.clamp(this.offsetY, rect.top, rect.bottom)
      e.srcElement.style.top = this.offsetY + 'px'
  }

  static pointerUp(e) {
    // stop moving
    e.srcElement.removeEventListener('pointermove', this.pointerMove)
    e.srcElement.releasePointerCapture(e.pointerId)
    const rect = e.srcElement.getBoundingClientRect()
    const rectHost = this.getHostRect()
   
    // 
    this.updateData(rect.left + 0.5 * rect.width - rectHost.left, rect.top + 0.5 * rect.height - rectHost.top)
  }

  static getHostRect = () => document.getElementById("host").getBoundingClientRect()

  static clamp = (val, min, max) => Math.min(Math.max(val, min), max)
}

window.DragPointHelper = DragPointHelper