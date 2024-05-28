/**
 * Returns list of recommended events
 * @async
 * @function getRecommendedEvents
 * @param {string} location - location of the user 'City, State, Country'
 * @returns {Promise<Array<object>>}
 */
export async function getRecommendedEvents(location) {
  let res = await fetch(`/api/RecommendationsApi/RecommendedEvents?location=${location}`)
  return await res.json();
}
