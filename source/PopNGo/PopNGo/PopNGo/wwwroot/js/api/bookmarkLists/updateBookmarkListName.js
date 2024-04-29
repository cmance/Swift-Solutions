/**
 * Update a bookmark list name
 * @async
 * @function updateBookmarkListName
 * @param {string} oldName
 * @param {string} newName
 * @returns {Promise<void>}
 */
export async function updateBookmarkListName(oldName, newName) {
  console.log("oldName: " + oldName);
  let url = "/api/BookmarkListApi/BookmarkList";
  const res = await fetch(url, {
      method: 'PUT',
      headers: {
          'Content-Type': 'application/json'
      },
      body: JSON.stringify({
          oldBookmarkListTitle: oldName,
          newBookmarkListTitle: newName
      })
  })

  if (!res.ok) {
      if (res.status === 401) {
          throw new Error('Unauthorized');
      }

      throw new Error('Network response was not ok');
  }
}
