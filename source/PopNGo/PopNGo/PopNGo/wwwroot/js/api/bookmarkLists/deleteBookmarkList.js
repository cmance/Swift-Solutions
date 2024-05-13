/**
 * Delete a bookmark list
 * @async
 * @function deleteBookmarkList
 * @param {string} bookmarkListName
 * @returns {Promise<void>}
 */
export async function deleteBookmarkList(bookmarkListName) {
  let url = "/api/BookmarkListApi/BookmarkList?listName=" + bookmarkListName;
  const res = await fetch(url, {
      method: 'DELETE',
      headers: {
          'Content-Type': 'application/json'
      },
  })

  if (!res.ok) {
      if (res.status === 401) {
          throw new Error('Unauthorized');
      }

      throw new Error('Network response was not ok');
  }
}
