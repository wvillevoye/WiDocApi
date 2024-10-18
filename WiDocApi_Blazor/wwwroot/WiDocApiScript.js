// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.


export function copyToClipboard(jsonObject) {
    if (!navigator.clipboard) {
        console.error("Clipboard API not supported.");
        return;
    }

    try {
        // Convert the jsonObject to a string and clean it in one step
        let cleanedString = JSON.stringify(jsonObject)
            .replace(/\r?\n/g, '') // Remove \r and \n characters
            .replace(/\\/g, '')    // Remove backslashes
            .replace(/rn/g, '')    // Remove literal "rn"
            .replace(/^"|"$/g, ''); // Remove starting and ending double quotes

        // Copy the cleaned string to the clipboard
        navigator.clipboard.writeText(cleanedString).then(() => {
            
        }).catch((err) => {
            console.error('Error copying text: ' + err);
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


export function formatJson(textAreaId) {
    const input = document.getElementById(textAreaId);

    if (!input) {
        console.error('No valid JSON text area found.');
        return;
    }

    try {
        const jsonObj = JSON.parse(input.value);
        const formattedJson = JSON.stringify(jsonObj, null, 2);
        input.value = formattedJson;
    } catch (error) {
        input.value = 'Invalid JSON: ' + error.message;
    }
}