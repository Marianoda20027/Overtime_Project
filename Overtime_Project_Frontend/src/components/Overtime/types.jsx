// src/pages/Overtime/types.js

/**
 * @typedef {Object} OvertimeForm
 * @property {string} date
 * @property {string} startTime
 * @property {string} endTime
 * @property {number} totalHours
 * @property {string} justification
 * @property {string} costCenter
 */

/**
 * @typedef {Object} OvertimeRequest
 * @property {string} id
 * @property {string} date
 * @property {number} totalHours
 * @property {string} justification
 * @property {'PENDING' | 'APPROVED' | 'REJECTED'} status
 */
