import type { OpenAIQueryCost } from '../../services/excel-analysis.service';

interface AIRequestCostProps {
	cost: OpenAIQueryCost | null;
}

function AIRequestCost({ cost }: AIRequestCostProps) {
	if (!cost) return null;

	return (
		<div className="mt-2">
			<span className="badge rounded-pill px-3 py-2 cost-badge-ai">
				💰 ${cost.totalCost} • {cost.inputTokenCount}→
				<span>{cost.outputTokenCount} </span>
				{cost.reasoningTokenCount > 0 && (
					<span>({cost.reasoningTokenCount}) </span>
				)}
				<span>tokens</span>
			</span>
		</div>
	);
}

export default AIRequestCost;
