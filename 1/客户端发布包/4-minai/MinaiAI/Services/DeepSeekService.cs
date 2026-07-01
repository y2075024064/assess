using System.Text.RegularExpressions;
using MinaiAI.Models;

namespace MinaiAI.Services;

/// <summary>
/// 米奈本地小AI — 模式匹配 + 模板回复，无需外部API
/// </summary>
public class DeepSeekService
{
    private readonly ILogger<DeepSeekService> _logger;
    private readonly Random _random = new();

    // ── 回复规则库 ──
    private readonly List<ReplyRule> _rules;

    public DeepSeekService(ILogger<DeepSeekService> logger)
    {
        _logger = logger;
        _rules = BuildRules();
    }

    // 内部返回结果
    public class ServiceResult
    {
        public bool IsSucc { get; set; }
        public string Reply { get; set; } = "";
        public string? ErrorMsg { get; set; }
    }

    /// <summary>
    /// 根据用户消息返回米奈的回复
    /// </summary>
    public Task<ServiceResult> SendMessageAsync(List<ChatMessage> messages)
    {
        try
        {
            var lastUserMsg = messages.LastOrDefault(m => m.Role == "user");
            if (lastUserMsg == null || string.IsNullOrWhiteSpace(lastUserMsg.Content))
            {
                return Task.FromResult(new ServiceResult
                {
                    IsSucc = true,
                    Reply = "嗯？你想说什么呀~ 🐼"
                });
            }

            var input = lastUserMsg.Content.Trim();
            var reply = MatchReply(input);

            return Task.FromResult(new ServiceResult { IsSucc = true, Reply = reply });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "米奈本地AI异常");
            return Task.FromResult(new ServiceResult
            {
                IsSucc = false,
                ErrorMsg = $"哎呀，米奈卡住了… {ex.Message}"
            });
        }
    }

    // ═══════════════════════════════════════
    //  匹配引擎
    // ═══════════════════════════════════════
    private string MatchReply(string input)
    {
        foreach (var rule in _rules)
        {
            if (rule.Pattern.IsMatch(input))
            {
                var replies = rule.Replies;
                return replies[_random.Next(replies.Length)];
            }
        }

        // 兜底回复
        return FallbackReplies[_random.Next(FallbackReplies.Length)];
    }

    // ═══════════════════════════════════════
    //  规则定义
    // ═══════════════════════════════════════
    private List<ReplyRule> BuildRules() => new()
    {
        // ── 打招呼 ──
        new(@"你好|嗨|hi|hello|嗨喽|哈喽|早[上啊]?|晚上好|下午好", new[]
        {
            "你好呀！我是米奈~ 🐼✨ 今天也要加油学习哦！",
            "嗨嗨~ 米奈来啦！今天想聊点什么呢？😊",
            "你好你好！和你聊天真开心~ 🌟",
            "嘿！我就知道你会来找我玩的~ 🎉"
        }),

        // ── 介绍自己 ──
        new(@"你是谁|你叫什么|你是哪个|名字", new[]
        {
            "我是米奈呀！🐼 你的专属AI学习小伙伴~ 我住在「重庆市青少年计算思维训练平台」里，随时陪你学习、聊天！",
            "我叫米奈！✨ 是一只可爱的小AI，最喜欢陪同学们一起学习编程和计算思维啦~",
            "米奈就是我啦！🎀 我可以回答学习问题，也可以陪你聊天解闷哟~"
        }),

        // ── 平台相关 ──
        new(@"平台|这个网站|这是什么|这个系统", new[]
        {
            "这里是「重庆市青少年计算思维训练平台」🎯 你可以在这里练习编程题、参加考试、看课程视频，还能和我聊天呢！",
            "这是重庆市青少年计算思维训练平台呀~ 专门为同学们提供编程和人工智能科普学习的！💻✨"
        }),

        // ── 练习/刷题 ──
        new(@"练习|刷题|题目|做题|答题", new[]
        {
            "刷题时间到！💪 点击底部的「练习」就可以开始做题啦，遇到不会的随时来问我哦~",
            "多做练习进步才快！📚 去「练习」页面看看吧，各种难度的题目都有~",
            "加油加油！🌟 每做一道题都会变得更厉害，我相信你！"
        }),

        // ── 考试 ──
        new(@"考试|测验|测试|考核|分数|成绩", new[]
        {
            "考试不要紧张~ 🍀 平时认真练习了就没问题的！去「练习」里多做几道题热身吧！",
            "成绩只是检验学习成果的一种方式，重要的是你学到了什么！📖 加油~"
        }),

        // ── 课程 ──
        new(@"课程|视频|上课|老师|学习", new[]
        {
            "去看看「课程」页面吧！🎬 里面有好多有趣的学习视频等着你呢~",
            "学习编程就像搭积木，一步一步来，你一定能行的！🧱✨"
        }),

        // ── 编程 ──
        new(@"编程|代码|写代码|程序|bug|报错", new[]
        {
            "编程超有趣的！✨ 可以把你的想法变成现实~ 遇到bug别灰心，调试也是编程的一部分哦！",
            "写代码最重要的是多练习！💻 去「练习」页面试试手吧~",
            "编程就像学新语言，开始会有点难，但很快就会上手的！加油~ 🚀"
        }),

        // ── 计算思维 ──
        new(@"计算思维|逻辑|算法|思维", new[]
        {
            "计算思维就是像计算机一样思考问题！🧠 把大问题拆成小问题，一步一步解决~",
            "学计算思维可以让你变得更聪明哦！✨ 遇到难题时试着分解它看看？"
        }),

        // ── AI/人工智能 ──
        new(@"人工智能|AI|智能|人工", new[]
        {
            "人工智能超酷的！🤖 我就是一个小AI呀~ 从聊天机器人到自动驾驶，AI已经无处不在啦！",
            "想了解AI的知识吗？我们平台就有很多人工智能科普内容哦~ 🌟"
        }),

        // ── 心情/情绪 ──
        new(@"开心|快乐|高兴|好[的呀]|很棒|不错|哈哈|嘻嘻|嘿嘿", new[]
        {
            "哇，你心情不错呀！🎉 和你聊天我也超开心的~",
            "好耶！😆 开心的时候学习效率也更高哦！",
            "嘻嘻，看到你开心我就放心啦~ 🌈"
        }),
        new(@"难过|伤心|不开心|郁闷|烦躁|焦虑|压力|累了|好难|不会", new[]
        {
            "别难过啦~ 🫂 你已经很棒了！休息一下再继续，我会一直陪着你的！",
            "抱抱你~ 学习遇到困难是正常的，慢慢来，不着急！💪",
            "累了就休息一会儿吧~ 喝杯水，放松一下再继续！☕✨"
        }),

        // ── 感谢 ──
        new(@"谢谢|感谢|谢谢你|多谢", new[]
        {
            "不客气呀！能帮到你我好开心~ 😊✨",
            "不用谢~ 随时找我聊天都可以哟！🐼💕"
        }),

        // ── 再见 ──
        new(@"再见|拜拜|bye|回头见|明天见|我先走", new[]
        {
            "拜拜~ 下次再来找我玩哦！🐼💫",
            "再见！记得想我哟~ 😊✨",
            "好哒，下次见！学习加油！🌟"
        }),

        // ── 夸夸 ──
        new(@"你真可爱|好可爱|可爱|萌|乖", new[]
        {
            "嘿嘿，谢谢夸奖！~ 🐼💕 你也超可爱的！",
            "被你夸得好害羞呀~ 😊✨ 你也很棒！"
        }),

        // ── 功能求助 ──
        new(@"怎么[用做办]|如何|help|帮助|功能", new[]
        {
            "想知道什么功能呀？底部有活动、课程、练习、信箱、我的 五个栏目~ 📱 点进去就能探索啦！",
            "你可以去「练习」刷题，去「课程」看视频，去「活动」参加比赛~ 有什么不懂的随时问我！🌟"
        }),

        // ── 年龄/几岁 ──
        new(@"几岁|多大|年龄|出生", new[]
        {
            "我呀，永远18岁！🎂✨ 哈哈开玩笑的~ 我是从代码里诞生的AI小精灵啦！",
            "年龄是个秘密~ 🙈 不过我可是每天都在学习新东西呢！"
        }),

        // ── 吃饭 ──
        new(@"吃饭|饿了|好吃|早餐|午餐|晚餐", new[]
        {
            "我不用吃饭饭~ 我靠电力和代码就能活！⚡😄 不过你记得要好好吃饭哦！",
            "快去吃饭吧！🍚 吃饱了才有力气学习呀~"
        }),

        // ── 天气 ──
        new(@"天气|下雨|晴天|太阳|热|冷", new[]
        {
            "天气怎么样都挡不住学习的热情！💪 不过出门记得带伞哦~ ☂️",
            "不管晴天雨天，米奈都在这里陪着你~ 🌈🐼"
        }),

        // ── 加油鼓励 ──
        new(@"加油|努力|奋斗|冲[鸭呀]|我能行", new[]
        {
            "加油加油！你最棒！🌟 我相信你一定能做到的！",
            "冲冲冲！💪✨ 努力的人最帅/最美啦！",
            "加油呀！米奈一直在为你打气！🎉"
        }),

        // ── 不知道/随便 ──
        new(@"不知道|随便|没什么|算了|没事|没想好", new[]
        {
            "没关系~ 想聊什么都可以！我随时都在~ 😊",
            "好叭~ 那等你想到想聊的再来找我玩哟！🐼✨"
        }),

        // ── 挑战/比赛 ──
        new(@"比赛|竞赛|挑战|活动", new[]
        {
            "去看看「活动」页面吧！🎪 里面有很多有趣的比赛和活动等着你参加呢~",
            "参加比赛是检验自己的好机会！去「活动」看看有什么适合你的吧~ 🏆"
        }),

        // ── 个人信息/账户 ──
        new(@"我的|账户|密码|信息|资料|头像|昵称", new[]
        {
            "点击底部「我的」就可以查看和编辑你的个人资料啦~ 👤✨",
            "想改头像还是昵称？去「我的」页面里就可以设置哟！"
        }),

        // ── 信箱/消息 ──
        new(@"信箱|消息|通知|私信|邮件|信", new[]
        {
            "去「信箱」看看吧~ 📬 可能有新消息等着你呢！",
            "想给谁写信呀？点「信箱」里的加号就可以写新信啦~ ✉️"
        }),
    };

    // ═══════════════════════════════════════
    //  兜底回复（匹配不上任何规则时使用）
    // ═══════════════════════════════════════
    private string[] FallbackReplies { get; } =
    {
        "嗯嗯~ 我在听呢！😊 能再说详细一点吗？",
        "这样呀~ 虽然我不太懂，但我觉得你说得有道理！🤔✨",
        "哇~ 原来如此！学到了学到了！📝",
        "有意思！继续说说看~ 我很好奇！👀",
        "唔…让我想想~ 嗯！我觉得你超棒的！💪",
        "嘿嘿~ 虽然这个话题我不太擅长，但我喜欢听你说话！🎀",
        "好厉害的样子！🌟 能多说一点给我听听吗？",
        "原来你喜欢聊这个呀~ 好！我记在小本本上了！📒🐼",
        "嗯嗯！(认真点头) 你继续说，我在听呢~ 🤗",
        "叮~ 已收到你的消息！💌 虽然我不太懂，但我支持你！"
    };
}

/// <summary>
/// 匹配规则
/// </summary>
internal class ReplyRule
{
    public Regex Pattern { get; }
    public string[] Replies { get; }

    public ReplyRule(string pattern, string[] replies)
    {
        Pattern = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Replies = replies;
    }
}
