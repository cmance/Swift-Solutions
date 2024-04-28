export async function getEmailMetrics(timeframe) {
    const response = await fetch(`/api/admin/metrics/emails/time=${timeframe}`);
    return await response.json();
}

export async function getAccountMetrics(timeframe) {
    const response = await fetch(`/api/admin/metrics/accounts/time=${timeframe}`);
    return await response.json();
}

export async function getEventMetrics(timeframe) {
    const response = await fetch(`/api/admin/metrics/searches/time=${timeframe}`);
    return await response.json();
}

export async function getUserCount() {
    const response = await fetch(`/api/admin/metrics/accounts/total`);
    return await response.text();
}