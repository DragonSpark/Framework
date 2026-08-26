if (!window.__sfTooltipFocusResetRegistered) {
    window.__sfTooltipFocusResetRegistered = true;

    const resetFocus = () => {
        const el = document.activeElement;
        if (el && el.classList && el.classList.contains('sf-tooltip-target')) {
            el.blur();
        }
    };

    window.addEventListener("scroll", resetFocus, true);
    window.addEventListener("blur", resetFocus, true);
}