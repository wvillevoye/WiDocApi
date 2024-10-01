// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.


export function copyToClipboard(jsonObject) {
    if (!navigator.clipboard) {
        console.error("Clipboard API not supported.");
        return;
    }

    // Convert the JSON object to a pretty-printed string without escape characters
    const text = JSON.stringify(jsonObject); // Indent with 2 spaces

  

    try {
        // Use the Clipboard API to copy
        navigator.clipboard.writeText(text).then(function () {
            alert('Copied to clipboard successfully!');
        }).catch(function (err) {
            alert('Error copying text: ', err);
        });
    } catch (err) {
        console.error('Error copying text: ', err);
    } finally {
        // Clean up the temporary text area
        document.body.removeChild(textArea);
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
