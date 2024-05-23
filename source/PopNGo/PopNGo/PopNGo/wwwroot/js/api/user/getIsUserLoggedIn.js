/**
 * Returns bool of if user is logged in
 * @async
 * @function getIsUserLoggedIn
 * @returns {Promise<boolean>}
 */
export async function getIsUserLoggedIn() {
  let res = await fetch(`/api/UserApi/LoggedIn`)
  return await res.json();
}
