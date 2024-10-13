// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.


export function copyToClipboard(jsonObject) {
    if (!navigator.clipboard) {
        console.error("Clipboard API not supported.");
        return;
    }

    const text = JSON.stringify(jsonObject, undefined, 2);
    try {
        navigator.clipboard.writeText(text).then(function () {
            alert('Copied to clipboard successfully!');
        }).catch(function (err) {
            alert('Error copying text: ' + err);
        });
    } catch (err) {
        console.error('Error copying text: ', err);
    }
}

export function downloadJsonFile(filename, jsonData) {
    const blob = new Blob([jsonData], { type: 'application/json' });
    const url = URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}
