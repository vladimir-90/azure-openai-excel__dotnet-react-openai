import type { AIQueryResult } from '../services/excel-analysis.service';

interface AIAnalysisResultProps {
	result: AIQueryResult;
}

function AIAnalysisResult({ result }: AIAnalysisResultProps) {
	return (
		<div className="container py-4">
			<div className="row">
				<div className="col-12">
					<div className="card">
						<div className="card-header">
							<h5 className="mb-0">Analysis Result</h5>
						</div>
						<div className="card-body">
							<div className="mb-3">
								<h6 className="text-muted">User Query:</h6>
								<p className="fw-semibold">
									{result.userQuery}
								</p>
							</div>

							<div className="mb-3">
								<h6 className="text-muted">Raw Response:</h6>
								<pre
									className="bg-light p-3 rounded border"
									style={{
										fontSize: '0.85rem',
										overflow: 'auto',
										maxHeight: '60vh',
									}}
								>
									{JSON.stringify(result, null, 2)}
								</pre>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}

export default AIAnalysisResult;
