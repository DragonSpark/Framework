if (!window.__sfTooltipFocusResetRegistered) {

    window.__sfTooltipFocusResetRegistered = true;



    const resetFocus = () => {

        const el = document.activeElement;

        if (el && typeof el.blur === "function") el.blur();

    };



    window.addEventListener("scroll", resetFocus, true);

    window.addEventListener("blur", resetFocus, true);

}