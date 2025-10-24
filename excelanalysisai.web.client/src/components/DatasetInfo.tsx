import { useState } from 'react';
import type { DataSetInfoDto } from '../services/generation-settings.service';
import Spinner from './base/Spinner';

interface DatasetInfoProps {
	datasetInfo: DataSetInfoDto | null;
	loading?: boolean;
}

function DatasetInfo({ datasetInfo, loading = false }: DatasetInfoProps) {
	const [showSchema, setShowSchema] = useState(false);
	const [showData, setShowData] = useState(false);

	if (loading) {
		return (
			<div className="mt-2 p-3 bg-light rounded-3 border">
				<div className="d-flex justify-content-center align-items-center gap-2">
					<Spinner sm />
					<small className="text-muted">
						Loading dataset info...
					</small>
				</div>
			</div>
		);
	}

	if (!datasetInfo) {
		return null;
	}

	return (
		<div className="mt-2 border rounded-3">
			{/* Data Preview Section */}
			<div className="border-bottom">
				<button
					className="btn btn-link text-start w-100 p-3 text-decoration-none"
					onClick={() => setShowData(!showData)}
					style={{ borderRadius: 0 }}
				>
					<div className="d-flex justify-content-between align-items-center">
						<span className="fw-semibold text-dark">
							🔍 Data Preview (First{' '}
							{datasetInfo.dataSlice.length} of{' '}
							{datasetInfo.totalEntityCount.toLocaleString()}{' '}
							rows)
						</span>
						<span
							className={`text-primary ${
								showData ? 'rotate-90' : ''
							}`}
						>
							▶
						</span>
					</div>
				</button>

				{showData && datasetInfo.dataSlice.length > 0 && (
					<div className="px-3 pb-3">
						<div
							className="table-responsive"
							style={{ maxHeight: '300px' }}
						>
							<table
								className="table table-sm table-striped table-hover"
								style={{ fontSize: '0.75rem' }}
							>
								<tbody>
									{datasetInfo.dataSlice.map(
										(row, rowIndex) => (
											<tr key={rowIndex}>
												{row.map((cell, cellIndex) => (
													<td
														key={cellIndex}
														className="text-truncate"
														style={{
															maxWidth: '150px',
														}}
														title={cell}
													>
														{cell}
													</td>
												))}
											</tr>
										)
									)}
								</tbody>
							</table>
						</div>
					</div>
				)}
			</div>

			{/* Schema Section */}
			<div>
				<button
					className="btn btn-link text-start w-100 p-3 text-decoration-none"
					onClick={() => setShowSchema(!showSchema)}
					style={{ borderRadius: 0 }}
				>
					<div className="d-flex justify-content-between align-items-center">
						<span className="fw-semibold text-dark">
							🏗️ Schema Information
						</span>
						<span
							className={`text-primary ${
								showSchema ? 'rotate-90' : ''
							}`}
						>
							▶
						</span>
					</div>
				</button>

				{showSchema && (
					<div className="px-3 pb-3">
						<pre
							className="bg-dark text-light p-3 rounded"
							style={{ fontSize: '0.7rem' }}
						>
							{datasetInfo.schema}
						</pre>
					</div>
				)}
			</div>
		</div>
	);
}

export default DatasetInfo;
