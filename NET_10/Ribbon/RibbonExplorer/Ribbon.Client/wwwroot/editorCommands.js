const editorStates = new WeakMap();

function isSelectionInside(editor, range) {
    return range && (range.commonAncestorContainer === editor || editor.contains(range.commonAncestorContainer));
}

function saveSelection(editor) {
    const selection = window.getSelection();
    if (!selection || selection.rangeCount === 0) {
        return;
    }

    const range = selection.getRangeAt(0);
    if (isSelectionInside(editor, range)) {
        editorStates.get(editor).range = range.cloneRange();
    }
}

function restoreSelection(editor) {
    const state = editorStates.get(editor);
    if (!state?.range) {
        return null;
    }

    const selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(state.range);
    return state.range;
}

function updateSavedRange(editor, range) {
    const selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(range);
    editorStates.get(editor).range = range.cloneRange();
}

export function initialize(editor, title, text) {
    const heading = document.createElement("h2");
    heading.textContent = title;
    const paragraph = document.createElement("p");
    paragraph.textContent = text;
    editor.replaceChildren(heading, paragraph);

    const state = { range: null, save: () => saveSelection(editor) };
    editorStates.set(editor, state);
    document.addEventListener("selectionchange", state.save);
}

export function dispose(editor) {
    const state = editorStates.get(editor);
    if (!state) {
        return;
    }

    document.removeEventListener("selectionchange", state.save);
    editorStates.delete(editor);
}

export async function execute(editor, command) {
    editor.focus();
    let range = restoreSelection(editor);

    if (command === "selectAll") {
        range = document.createRange();
        range.selectNodeContents(editor);
        updateSavedRange(editor, range);
        return true;
    }

    if (command === "undo" || command === "redo" || command === "bold" || command === "removeFormat") {
        const executed = document.execCommand(command);
        saveSelection(editor);
        return executed;
    }

    if (!range || !isSelectionInside(editor, range)) {
        return false;
    }

    if (command === "copy" || command === "cut") {
        if (range.collapsed) {
            return false;
        }

        const executed = document.execCommand(command);
        saveSelection(editor);
        return executed;
    }

    if (command === "paste") {
        if (!navigator.clipboard) {
            return false;
        }

        let text;
        try {
            text = await navigator.clipboard.readText();
        } catch {
            return false;
        }
        const executed = document.execCommand("insertText", false, text);
        saveSelection(editor);
        return executed;
    }

    if (command === "delete") {
        if (range.collapsed) {
            return false;
        }

        const executed = document.execCommand("delete");
        saveSelection(editor);
        return executed;
    }

    return false;
}
