import { capitalizeFirstLetter } from "./util/capitalizeFirstLetter.js";


export async function fetchTagColor(tag) {
    try {
        const response = await fetch(`/api/tags/name=${tag}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function createTags(events) {
    if(events.length === 0) return;

    // Create a set of unique tags across the events
    let tagList = new Set();
    events?.forEach(event => {
        // Check if event is not null before accessing its properties
        if (event) {
            event.eventTags?.forEach(tag => {
                tag = capitalizeFirstLetter(tag).replace(/-|_/g, ' ');
                tagList.add(tag);
            });
        }
    });

    if(tagList.size === 0) return;

    // Create the tags
    try {
        const response = await fetch(`/api/tags/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Array.from(tagList))
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        throw error;
    }
}

export async function formatTags(tags, tagsParent) {
    processArray(tags, async (tag) => {
        tag = capitalizeFirstLetter(tag).replace(/-|_/g, ' ');

        const tagEl = document.createElement('span');
        tagEl.classList.add('tag');

        let tagStyle = await fetchTagColor(tag) || null;
        if(tagStyle !== null) {
            tagEl.style.backgroundColor = tagStyle.backgroundColor;
            tagEl.style.color = tagStyle.textColor;
        }

        tagEl.textContent = tag;
        tagsParent.appendChild(tagEl);
    }).then(() => {
        // Grab the children we just made
        let children = Array.prototype.slice.call(tagsParent.children);

        // Sort the children elements
        children.sort((a, b) => a.textContent.localeCompare(b.textContent));

        // Append each child back to tags
        children.forEach(child => tagsParent.appendChild(child));
    });
}

export async function processArray(array, asyncFunction) {
    // map array to promises
    const promises = array.map(asyncFunction);
    // wait until all promises resolve
    await Promise.all(promises);
}
