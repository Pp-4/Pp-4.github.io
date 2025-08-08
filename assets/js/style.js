let index = 0;
document.querySelectorAll('svg.dither').forEach((svg) => {
    const baseColor1 = svg.getAttribute('data-color1');
    const baseColor2 = svg.getAttribute('data-color2');
    svg.querySelectorAll('rect').forEach((rect) => {
        let color1 = rect.getAttribute('data-color1');
        let color2 = rect.getAttribute('data-color2');

        //use parent colors if not specified
        if (!color1) color1 = baseColor1;
        if (!color2) color2 = baseColor2;

        // clone the original pattern
        const url = rect.getAttribute('fill');
        const patternName = url.substring(4, url.length - 1);
        const pattern = document.querySelector(patternName).cloneNode(true);
        pattern.id = `${pattern.id}-${index}`;
        index = index + 1;

        // set rect fill to match the new pattern id
        rect.setAttribute('fill', `url(#${pattern.id})`);

        // change colors in the cloned pattern
        pattern.querySelectorAll('.rect-color-1').forEach(rect => rect.setAttribute('fill', color1));
        pattern.querySelectorAll('.rect-color-2').forEach(rect => rect.setAttribute('fill', color2));

        // add cloned pattern to the current svg
        let defs = document.createElementNS("http://www.w3.org/2000/svg", 'defs');
        defs.appendChild(pattern);
        svg.appendChild(defs);
    })
});