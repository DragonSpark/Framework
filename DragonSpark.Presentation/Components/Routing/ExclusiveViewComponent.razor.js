export class ExclusiveViewComponent {
    constructor(dotNetHelper, channelName = 'exclusive_view_component_lock') {
        this.dotNetHelper = dotNetHelper;
        this.channelName = channelName;
        this.channel = new BroadcastChannel(channelName);
        this.flashInterval = null;

        const rand = Math.random().toString(36);
        this.tabId = rand.substring(2);

        this.isPrimary = true;

        this.channel.onmessage = (event) => {
            const { type, senderId } = event.data;

            if (type === 'WHO_IS_PRIMARY' && this.isPrimary) {
                this.channel.postMessage({ type: 'I_AM_PRIMARY', senderId: this.tabId });
            }
            else if (type === 'I_AM_PRIMARY' && senderId !== this.tabId) {
                this.isPrimary = false;
                this.dotNetHelper.invokeMethodAsync('HandleLockOut');
            }
            else if (type === 'FOCUS_REQUEST' && this.isPrimary) {
                // Browsers block window.focus() here, so we flash the tab instead
                this.flashTab();
            }
        };

        this.claimPrimary();
    }

    flashTab() {
        if (this.flashInterval) return; // Already flashing

        const originalTitle = document.title;
        let isFlash = false;

        // Toggle the title every 500ms
        this.flashInterval = setInterval(() => {
            document.title = isFlash ? "👋 Existing Tab Here!" : originalTitle;
            isFlash = !isFlash;
        }, 500);

        // Stop flashing as soon as the user interacts with the primary tab
        const stopFlashing = () => {
            if (this.flashInterval) {
                clearInterval(this.flashInterval);
                this.flashInterval = null;
                document.title = originalTitle;

                // Clean up listeners
                window.removeEventListener('focus', stopFlashing);
                window.removeEventListener('mousemove', stopFlashing);
            }
        };

        window.addEventListener('focus', stopFlashing);
        window.addEventListener('mousemove', stopFlashing);
    }

    claimPrimary() {
        this.isPrimary = true;
        this.channel.postMessage({ type: 'WHO_IS_PRIMARY', senderId: this.tabId });
    }

    requestPrimary() {
        this.claimPrimary();
    }

    focusExisting() {
        this.channel.postMessage({ type: 'FOCUS_REQUEST', senderId: this.tabId });
    }

    dispose() {
        this.channel.close();
    }
}

export function createLock(dotNetHelper, channelName) {
    return new ExclusiveViewComponent(dotNetHelper, channelName);
}